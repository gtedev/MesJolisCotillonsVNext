using MesJolisCotillons.Extensions.Paypal;
using MesJolisCotillons.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Xml.Serialization;
using MesJolisCotillons.Extensions;
using System.Collections.Generic;

namespace MesJolisCotillons.Area.PaypalNotification.Controllers
{
    public class IPNController : Controller
    {

        //Command_Repository command_Repository = new Command_Repository();

        [HttpPost]
        public HttpStatusCodeResult Receive(PaypalNotificationForm form)
        {
            string jsonString = new JavaScriptSerializer().Serialize(form);
            //var param = Request.BinaryRead(Request.ContentLength);
            //var strRequest = Encoding.ASCII.GetString(param);
            //sendIPNTestEmailConfirmation("Param:" + strRequest);
            sendIPNTestEmailConfirmation(jsonString);

            //Store the IPN received from PayPal
            LogRequest(Request, form);

            //Fire and forget verification task
            Task.Run(() => VerifyTask(Request, form));

            //Reply back a 200 code
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void VerifyTask(HttpRequestBase ipnRequest, PaypalNotificationForm form)
        {
            var verificationResponse = string.Empty;

            try
            {
                var ipnListenerUrl = PaypalConfig.PaypalConfigDependingOnMode["IpnListenerUrl"];
                //var verificationRequest = (HttpWebRequest)WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");

                var verificationRequest = (HttpWebRequest)WebRequest.Create(ipnListenerUrl);
                var protocolCurrent = ServicePointManager.SecurityProtocol;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                #region Set values for the verification request
                verificationRequest.Method = "POST";
                verificationRequest.ContentType = "application/x-www-form-urlencoded";
                var param = Request.BinaryRead(ipnRequest.ContentLength);
                var strRequest = Encoding.ASCII.GetString(param);
                #endregion

                #region Add cmd=_notify-validate to the payload                
                strRequest = "cmd=_notify-validate&" + strRequest;
                sendIPNTestEmailConfirmation(strRequest);
                verificationRequest.ContentLength = strRequest.Length;
                #endregion

                #region Attach payload to the verification request
                var streamOut = new StreamWriter(verificationRequest.GetRequestStream(), Encoding.ASCII);
                streamOut.Write(strRequest);
                streamOut.Close();
                #endregion

                #region Send the request to PayPal and get the response
                var streamIn = new StreamReader(verificationRequest.GetResponse().GetResponseStream());
                verificationResponse = streamIn.ReadToEnd();
                streamIn.Close();
                #endregion

                //verificationResponse = "VERIFIED";
                ProcessVerificationResponse(verificationResponse, form);

                ServicePointManager.SecurityProtocol = protocolCurrent;
            }
            catch (Exception exception)
            {
                //Capture exception for manual investigation
                sendIPNTestEmailConfirmation("VerifyTask:" + exception.Message);
            }
        }

        private void LogRequest(HttpRequestBase request, PaypalNotificationForm form)
        {
            // Persist the request values into a database or temporary data store
        }

        private void ProcessVerificationResponse(string verificationResponse, PaypalNotificationForm form)
        {
            try
            {
                if (verificationResponse.Equals("VERIFIED"))
                {
                    sendIPNTestEmailConfirmation("VERIFIED");
                    processVerifiedPayment(form);
                }
                else if (verificationResponse.Equals("INVALID"))
                {
                    sendIPNTestEmailConfirmation("INVALID");
                    //Log for manual investigation
                }
                else
                {
                    sendIPNTestEmailConfirmation("ELSE...");
                    //Log error
                }
            }
            catch (Exception e)
            {
                sendIPNTestEmailConfirmation("ProcessVerificationResponse:" + e.Message);
                sendIPNTestEmailConfirmation("ProcessVerificationResponse - InnerException:" + e.InnerException);
                sendIPNTestEmailConfirmation("ProcessVerificationResponse- StackTrace:" + e.StackTrace);
            }

        }

        private void processVerifiedPayment(PaypalNotificationForm form)
        {
            var baseUrl = PaypalConfig.PaypalConfigDependingOnMode["BaseUrl"];
            Command_Repository command_Repository = new Command_Repository();
            int commanId;
            var isParseOk = Int32.TryParse(form.custom, out commanId);
            if (isParseOk)
            {
                var command = command_Repository.FindCommand(commanId);
                if (command == null)
                {
                    return; //register in log file
                }

                #region Handling Command Status, and History
                switch (form.PaymentStatus)
                {
                    case PaymentStatus.Completed:

                        if (isPaypalConditionsRespected(form, command))
                        {
                            if (command.Command_Status == Command_Status.Command_Expired)
                            {
                                command.sendWarningEmailForPaymentOnExpiredCommand(baseUrl);
                                command.addCommand_History(Command_History_Action.Command_Expired, "Paiement Paypal reçu alors que la commande a expirée, un mail a été envoyée");
                            }
                            else
                            {
                                command.Command_Status = Command_Status.Command_Paid;
                                command.addCommand_History(Command_History_Action.Paypal_IPN_Notification_PaymentSuccess, "Notification paypal est un succès, le paiement est confirmé", Paypal_Txn_Id: form.txn_id);
                                command.addCommand_History(Command_History_Action.Command_Paid, "Le statut de la commande est passée à Commande payée");
                                command.sendEmailPaymentConfirmationToUser(baseUrl);
                                command.sendEmailPaymentConfirmationForAdministrators(baseUrl);
                            }

                            //string baseUrl = this.Request.Url.GetLeftPart(UriPartial.Authority);

                            // Download facture
                        }
                        break;
                    case PaymentStatus.Expired:
                        command.Command_Status = Command_Status.Command_Expired;
                        command.rechargeStockQuantity();

                        command.addCommand_History(Command_History_Action.Paypal_IPN_Notification_PaymentExpired, "Notification paypal informe que la transaction liée a expirée, la commande est annulée", Paypal_Txn_Id: form.txn_id);
                        command.addCommand_History(Command_History_Action.Command_Expired, "Le statut de la commande est passée à Commande Expirée");
                        break;
                    case PaymentStatus.Declined:
                        command.Command_Status = Command_Status.Command_Declined;
                        command.rechargeStockQuantity();

                        command.addCommand_History(Command_History_Action.Paypal_IPN_Notification_PaymentDeclined, "Notification paypal informe que la transaction a été déclinée, la commande est annulée", Paypal_Txn_Id: form.txn_id);
                        command.addCommand_History(Command_History_Action.Command_Declined, "Le statut de la commande est passée à Commande Expirée");
                        break;
                }
                #endregion

                #region Delete PaypalButton if Hosted
                var commandHistory = command.Command_History_Set.AsEnumerable()
                                                                .Where(item => item.Command_History_Action == Command_History_Action.Paypal_CreationPaymentButton && item.Command_History_XmlData.IsPaypalCreationButtonAliveHosted).FirstOrDefault();

                if (commandHistory != null)
                {
                    var hostedButtonId = commandHistory?.Command_History_XmlData?.PaypalHostedButtonID;

                    var paypalApi = new PaypalAPI();
                    paypalApi.deletePaymentButton(hostedButtonId);
                    //commandHistory.SetStatusPaypalHostedButton(PaypalHostedButtonStatus.DELETED);

                    var paypalButtonInfo = commandHistory.Command_History_XmlData.paypalButtonInfos;
                    paypalButtonInfo.Status = PaypalHostedButtonStatus.DELETED;
                    command.addCommand_History(Command_History_Action.Paypal_CreationPaymentButton, "Boutton de transaction Paypal supprimé avec succès", PaypalCreationlButtonInfo: paypalButtonInfo);

                    sendIPNTestEmailConfirmation("Delete PaypalButton if Hosted HostedID:" + hostedButtonId);
                }
                #endregion

                command.setPaypalNotificationFormCommandXmlData(form);
                sendIPNTestEmailConfirmation("After setPaypalNotificationFormCommandXmlData");

                command_Repository.Save();

            }
        }

        private bool isPaypalConditionsRespected(PaypalNotificationForm form, Command command)
        {

            #region Ontology conditions
            // check that Payment_status=Completed
            // check that Txn_id has not been previously processed
            // check that Receiver_email is your Primary PayPal email
            // check that Payment_amount/Payment_currency are correct
            // process payment 
            #endregion

            var isTxnIdAlreadyProcessed = CheckifTxnIdAlreadyProcessed(form.txn_id);
            var facilitatorEmail = PaypalConfig.PaypalConfigDependingOnMode["facilitatorEmail"];
            decimal paymentAmount = 0;
            //var isParseOk = Decimal.TryParse(form.mc_gross, out paymentAmount);
            var isParseOk = Decimal.TryParse(form.mc_gross, NumberStyles.Any, new CultureInfo("en-US"), out paymentAmount);

            //var test = form.payment_status == "Completed" && form.receiver_email == facilitatorEmail && isParseOk && command.TotalPrice == paymentAmount && form.mc_currency == "EUR" && !isTxnIdAlreadyProcessed;
            //sendIPNTestEmailConfirmation("isPaypalConditionsRespected:" + test.ToString());

            return form.payment_status == "Completed" && form.receiver_email == facilitatorEmail && isParseOk && command.TotalPrice == paymentAmount && form.mc_currency == "EUR" && !isTxnIdAlreadyProcessed;
        }

        private bool CheckifTxnIdAlreadyProcessed(string txn_id)
        {
            Command_Repository command_Repository = new Command_Repository();

            #region old
            //var allPaypalCommandHistory = command_Repository.FindAllCommand_History().Where(item => item.Command_History_Action == Command_History_Action.Paypal_IPN_Notification_PaymentSuccess ||
            //item.Command_History_Action == Command_History_Action.Paypal_IPN_Notification_PaymentFailed ||
            //item.Command_History_Action == Command_History_Action.Paypal_IPN_Notification_PaymentDeclined);

            //var allPaypalCommandHistoryXml = allPaypalCommandHistory.AsEnumerable().Select(item => item.Command_History_XmlData);

            //return allPaypalCommandHistoryXml.Any(item => item.Paypal_Txn_Id != null && item.Paypal_Txn_Id == txn_id); 
            #endregion
            //sendIPNTestEmailConfirmation("CheckifTxnIdAlreadyProcessed: before");

            //var test = command_Repository.FindAllCommand()
            //                         .AsEnumerable()
            //                         .Any(item => item?.Command_XmlData?.PaypalNotificationForm?.txn_id == txn_id);

            //sendIPNTestEmailConfirmation("CheckifTxnIdAlreadyProcessed: after");

            return command_Repository.FindAllCommand()
                                     .AsEnumerable()
                                     .Any(item => item?.Command_XmlData?.PaypalNotificationForm?.txn_id == txn_id);

        }


        #region test method
        public ActionResult LocalIpn()
        {
            return View("~/Views/IPN/IPNFormTestView.cshtml");
        }

        private void sendIPNTestEmailConfirmation(string bodyContent)
        {
            var debugMode = PaypalConfig.PaypalConfigDependingOnMode["PaypalNotificationsMailDebugMode"];
            if (debugMode == "false")
            {
                return;
            }

            string eMail = "gerard.te.85@gmail.com";
            Util.sendEmail(bodyContent, "IPN Mail test", eMail);
        }
        #endregion
    }
}


#region Useful class, enum
namespace MesJolisCotillons.Models
{
    public class PaypalNotificationForm
    {
        public string payment_status { get; set; }
        public string payment_date { get; set; }
        public string address_status { get; set; }
        public string payer_status { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string payer_email { get; set; }
        public string payer_id { get; set; }
        public string address_name { get; set; }
        public string address_country { get; set; }
        public string address_country_code { get; set; }
        public string address_zip { get; set; }
        public string address_state { get; set; }
        public string address_city { get; set; }
        public string address_street { get; set; }
        public string business { get; set; }
        public string receiver_email { get; set; }
        public string receiver_id { get; set; }
        public string residence_country { get; set; }
        public string item_amount { get; set; }
        public string item_name { get; set; }
        public string item_number { get; set; }
        public string quantity { get; set; }
        public string shipping { get; set; }
        public string tax { get; set; }
        public string mc_currency { get; set; }
        public string mc_fee { get; set; }
        public string mc_gross { get; set; }
        public string mc_gross_1 { get; set; }
        public string txn_type { get; set; }
        public string txn_id { get; set; }
        public string notify_version { get; set; }
        public string receipt_ID { get; set; }
        public string custom { get; set; }
        public string invoice { get; set; }

        [XmlIgnore]
        public PaymentStatus PaymentStatus
        {
            get
            {
                PaymentStatus result = PaymentStatus.Undefined;

                switch (payment_status)
                {
                    case "Completed":
                        result = PaymentStatus.Completed;
                        break;
                    case "Expired":
                        result = PaymentStatus.Expired;
                        break;
                    case "Declined":
                        result = PaymentStatus.Declined;
                        break;
                }

                return result;
            }
        }
    }
    public enum PaymentStatus : int
    {
        Undefined = 0,
        Declined = -200,
        Expired = -100,
        Completed = 200,
    }
}

#endregion
