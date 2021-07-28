using MesJolisCotillons.Area.Controllers;
using MesJolisCotillons.Extensions;
using MesJolisCotillons.Extensions.Paypal;
using MesJolisCotillons.ViewModels;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using static MesJolisCotillons.Area._Admin.Controllers.Direct.CommandDirectController;

namespace MesJolisCotillons.Models
{
    public partial class Command
    {
        private Command_XmlData _commandXmlData;
        public decimal? TotalCommand
        {
            get
            {
                decimal? total = 0;
                if (this?.CommandProduct_Set?.Count > 0)
                {
                    total = this.CommandProduct_Set?
                            .Select(item => item.TotalCommandProduct)
                            .Aggregate((a, b) => a + b);
                }

                return total;
            }
        }

        public CommandAddress CommandAddressShipment
        {
            get
            {
                var result = this.CommandAddress_Set.Where(a => a.Type.HasFlag(CommanAddress_Type.Shipment))
                                                  .FirstOrDefault();

                return result;
            }
        }

        public Command_History addCommand_History(Command_History_Action Action, string Description, string CommentFromAdminUser = null, DateTime? ShipmentDate = null, decimal? FeesPaidInPostOffice = null, CommandActionRadioForcePayment? ForcePaymentReason = null, string ColissimoNumber = null, string Paypal_Txn_Id = null, PaypalCreatedButtonInfos PaypalCreationlButtonInfo = null)
        {
            var historyXmlData = new Command_History_XmlData
            {
                Description = Description
            };

            if (!String.IsNullOrEmpty(CommentFromAdminUser))
            {
                historyXmlData.CommentFromAdminUser = CommentFromAdminUser;
            }

            if (ShipmentDate != null)
            {
                historyXmlData.ShipmentDate = ShipmentDate;
            }

            if (FeesPaidInPostOffice != null)
            {
                historyXmlData.FeesPaidInPostOffice = FeesPaidInPostOffice;
            }
            if (ForcePaymentReason != null)
            {
                historyXmlData.ForcePaymentReason = ForcePaymentReason;
            }

            if (ColissimoNumber != null)
            {
                historyXmlData.ColissimoNumber = ColissimoNumber;
            }

            if (Paypal_Txn_Id != null)
            {
                historyXmlData.Paypal_Txn_Id = Paypal_Txn_Id;
            }

            if (PaypalCreationlButtonInfo != null)
            {
                historyXmlData.paypalButtonInfos = PaypalCreationlButtonInfo;
            }

            var history = new Command_History
            {
                Date = DateTime.Now,
                Command_History_Action = Action,
                xmlData = historyXmlData.Serialize()
            };

            this.Command_History_Set.Add(history);

            return history;
        }

        public void rechargeStockQuantity()
        {
            var productRepository = new Product_Repository();
            //DeliveryCharge = 0;

            foreach (var item in this.CommandProduct_Set.ToList())
            {
                var productEntity = item.Product;
                if (productEntity != null)
                {
                    if (productEntity.Product_XmlData.ProductType == ProductType.SERIE)
                    {
                        productEntity.StockQuantity += item.Quantity;
                    }
                    else if (productEntity.Product_XmlData.ProductType == ProductType.UNIQUE)
                    {
                        productEntity.StockQuantity = 1;
                    }
                }

                //this.CommandProduct_Set.Remove(item);
            }
        }
        public Command_XmlData Command_XmlData
        {
            get
            {
                if (_commandXmlData == null)
                {
                    _commandXmlData = Command_XmlData.Deserialize<Command_XmlData>(this.xmlData);
                }

                return _commandXmlData;
            }
        }
        public void setPaypalNotificationFormCommandXmlData(PaypalNotificationForm paypalNotificationForm)
        {
            var commandXmlData = new Command_XmlData
            {
                PaypalNotificationForm = paypalNotificationForm
            };

            this.xmlData = commandXmlData.Serialize();
        }
        public void setActionChoiceCommandCommandXmlData(CommandActionForm actionForm)
        {
            var commandXmlData = this.Command_XmlData;
            commandXmlData.CommentFromAdminUser = actionForm.CommentFromAdminUser;
            commandXmlData.ShipmentDate = actionForm.ShipmenDate;
            commandXmlData.CommandChoiceAction = actionForm.CommandChoiceAction;

            if (actionForm.FeesPaidInPostOfficeField != null)
            {
                commandXmlData.FeesPaidInPostOffice = actionForm.FeesPaidInPostOfficeField;
            }
            if (actionForm.ColissimoNumber != null)
            {
                commandXmlData.ColissimoNumber = actionForm.ColissimoNumber;
            }

            this.xmlData = commandXmlData.Serialize();
        }

        public void sendEmailPaymentConfirmationToUser(string baseUrl)
        {
            string subjectEmail = "Mes Jolis Cotillons - Confirmation de votre commande";
            string introMsg = "<h4>Bonjour, </h4><br>" + "Nous vous remercions pour votre commande sur le site de Mes Jolis Cotillons. <br> Vous trouverez ci-dessous le détail de votre commande<br><br>";

            string endingMsg = "<br><br><br><br>Vous recevrez un message de confirmation lors de l'envoi de votre colis. <br><br> A très bientôt sur notre site, <br><b>L'équipe Mes Jolis Cotillons</b><br><br>";

            var email = Customer_User.User.eMail;
            var listEmail = new List<string> { email };
            sendEmailPaymentConfirmation(baseUrl, subjectEmail, introMsg, endingMsg, listEmail);
        }
        public void sendWarningEmailForPaymentOnExpiredCommand(string baseUrl)
        {

            #region subjectEmail
            var subjectEmail = "[Warning] Mes Jolis Cotillons - Commande payée sur une commande Expirée (Commande N°:" + this.Command_ID + ")";
            #endregion

            #region bodyContent
            string bodyContent = "<h4>Bonjour, </h4><br>" + "Nous avons reçu un paiement sur une commande qui a été expirée <br>" + "<div> Faîtes l'action approprié, un remboursement peut être une eventualité.</div><br>";

            var commandSummary = "<div>Récapitulatif de la commande concernée (Commande N°:" + this.Command_ID + ")</div>";

            var viewPath = "~/Views/Customer/CommandDetailsConfirmation.cshtml";
            var commandViewModel = new CommandViewModel
            {
                BaseUrl = baseUrl,
                Command = this
            };

            var result = Util.RenderViewToString(viewPath, commandViewModel);


            bodyContent += commandSummary + "<br>" + "<br>" + result;

            var shipmentAddress = CommandAddressShipment;

            if (shipmentAddress != null)
            {
                var addressHtml = "<div><b>Adresse de livraison:</b></div><br>";
                addressHtml += shipmentAddress.FullName + "<br>";
                addressHtml += shipmentAddress.Address.Address1 + "<br>";
                addressHtml += shipmentAddress.Address.Zip_Code + "<br>";
                addressHtml += shipmentAddress.Address.City + "<br>";
                addressHtml += shipmentAddress.Address?.Country.Name;
                bodyContent += addressHtml;
            }

            bodyContent += "<br><br><br><br>A très bientôt, <br><br>";
            bodyContent += "<b>Le robot Mes Jolis Cotillons</b><br><br>";
            #endregion

            var adminEmail_List = Settings.AdministratorsEmails_List;
            Util.sendEmail(bodyContent, subjectEmail, adminEmail_List);
        }
        public void sendEmailPaymentConfirmationForAdministrators(string baseUrl)
        {
            string customerEmail = Customer_User.User.eMail;
            string customerFullName = Customer_User.User.FullName;

            string subjectEmail = "Mes Jolis Cotillons - Commande N° " + this.Command_ID + " à préparer";
            string introMsg = "<h4>Bonjour, </h4><br>" + "Le client " + customerFullName + " (" + customerEmail + ") a passé une commande sur le site de Mes Jolis Cotillons. <br> Ci-dessous le détail de sa commande<br><br>";
            string endingMsg = "<br><br><br><br>Il n'y a plus qu'à préparer l'envoi de votre colis :) Vous pouvez allez sur le lien suivant pour consulter les commandes du site sur la partie Administration: <a href=\"" + baseUrl + "/Admin\">Administration</a> <br><br><br><br>A très bientôt, <br><br><b>Le robot Mes Jolis Cotillons</b><br><br>";
            var adminEmail_List = Settings.AdministratorsEmails_List;

            sendEmailPaymentConfirmation(baseUrl, subjectEmail, introMsg, endingMsg, adminEmail_List);
        }
        private void sendEmailPaymentConfirmation(string baseUrl, string subjectEmailParam, string introMsg, string endingMsg, List<string> email_List)
        {

            #region subjectEmail
            var subjectEmail = subjectEmailParam;
            #endregion

            #region bodyContent
            string bodyContent = introMsg;

            var commandSummary = "<div>Récapitulatif de la commande (Commande N°:" + this.Command_ID + ")</div>";

            var viewPath = "~/Views/Customer/CommandDetailsConfirmation.cshtml";
            var commandViewModel = new CommandViewModel
            {
                BaseUrl = baseUrl,
                Command = this
            };

            var commandDetailsConfirmationViewString = Util.RenderViewToString(viewPath, commandViewModel);


            bodyContent += commandSummary + "<br>" + "<br>" + commandDetailsConfirmationViewString;

            var typeShipment = ShipmentType != null ? ((ShipmentType)ShipmentType).ToStringFrench() : " - ";
            if (ShipmentType != null && ShipmentType == Models.ShipmentType.OptionSecureShipment)
            {
                typeShipment += " (" + OptionShipmentCharge + "€)";
            }
            bodyContent += "<div><b>Type de livraison:</b> " + typeShipment + " </div><br>";

            if (ShipmentType == Models.ShipmentType.NormalShipment || ShipmentType == Models.ShipmentType.OptionSecureShipment)
            {
                var shipmentAddress = CommandAddressShipment;
                if (shipmentAddress != null)
                {
                    var addressHtml = "<div><b>Adresse de livraison:</b></div><br>";
                    addressHtml += shipmentAddress.FullName + "<br>";
                    addressHtml += shipmentAddress.Address.Address1 + "<br>";
                    addressHtml += shipmentAddress.Address.Zip_Code + "<br>";
                    addressHtml += shipmentAddress.Address.City + "<br>";
                    addressHtml += shipmentAddress.Address?.Country.Name;
                    bodyContent += addressHtml;
                }
            }

            bodyContent += endingMsg;
            #endregion

            Util.sendEmail(bodyContent, subjectEmail, email_List);
        }
    }
    public partial class CommandProduct
    {

        private CommandProduct_XmlData _commandProductXmlData = null;

        public CommandProduct_XmlData CommandProduct_XmlData
        {
            get
            {
                if (_commandProductXmlData == null)
                {
                    _commandProductXmlData = CommandProduct_XmlData.Deserialize<CommandProduct_XmlData>(this.xmlData);
                }

                return _commandProductXmlData;
            }
        }

        public decimal? TotalCommandProduct
        {
            get
            {
                return this.Product.Price * this.Quantity;
            }
        }
    }

    public partial class CommandAddress
    {
        public string FullName
        {
            get
            {
                return this.FirstName + ", " + this.LastName;
            }
        }
    }
    public partial class Command_History
    {
        private Command_History_XmlData _command_HistoryXmlData = null;
        public Command_History_XmlData Command_History_XmlData
        {
            get
            {
                if (_command_HistoryXmlData == null)
                {
                    _command_HistoryXmlData = Command_History_XmlData.Deserialize<Command_History_XmlData>(this.xmlData);
                }

                return _command_HistoryXmlData;
            }
        }
        public void SetStatusPaypalHostedButton(PaypalHostedButtonStatus status)
        {
            var commandHistoryXmlData = this.Command_History_XmlData;

            if (this.Command_History_Action == Command_History_Action.Paypal_CreationPaymentButton && commandHistoryXmlData.IsPaypalCreationButtonAliveHosted && commandHistoryXmlData.paypalButtonInfos != null)
            {
                commandHistoryXmlData.paypalButtonInfos.Status = status;
                this.xmlData = commandHistoryXmlData.Serialize();
            }
        }
    }
}