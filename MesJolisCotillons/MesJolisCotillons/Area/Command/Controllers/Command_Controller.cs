using MesJolisCotillons.Area.Controllers;
using MesJolisCotillons.Extensions.LaPoste;
using MesJolisCotillons.Extensions.Paypal;
using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MesJolisCotillons.Area.Controllers
{
    public class CommandController : Controller
    {

        Command_Repository command_Repository = new Command_Repository();
        LaPosteColissimo_Repository laPoste_Repository = new LaPosteColissimo_Repository();

        //public string deletePaypalButton(string id)
        //{
        //    var api = new PaypalAPI();
        //    api.deletePaymentButton(id);

        //    return "true";
        //}

        #region View
        //[CustomerAuthorize]
        public ActionResult ProcessCommand()
        {

            var command = Session["Command"];
            MySessionCartModel myCart = null;
            if (command != null)
            {
                myCart = (MySessionCartModel)command;
                if (myCart.commandProducts.Count() > 0)
                {
                    decimal deliveryCharge = computeDeliveryChargeForCart(myCart);
                    myCart.DeliveryCharge = deliveryCharge;

                    return View("~/Views/Command/MyCommand.cshtml", myCart);
                }
            }

            return Redirect("/");
        }
        #endregion

        #region Partial View
        //[CustomerAuthorize]

        [AjaxOrChildActionOnly]
        public ActionResult CommandLoginPartialView()
        {
            return PartialView("~/Views/Command/CommandLogin.cshtml");
        }

        [AjaxOrChildActionOnly]
        public ActionResult CommandAddressInvoicePartialView()
        {
            var model = new CustomerAccountViewModel
            {
                CustomerUser_FK = CurrentUser.User?.User_ID,
                Address_FK = CurrentUser.User?.Customer_User?.CustomerAddressInvoice?.Address?.Address_ID
            };

            return PartialView("~/Views/Command/CommandAddressInvoice.cshtml", model);
        }

        //[CustomerAuthorize]
        [AjaxOrChildActionOnly]
        public ActionResult CommandAddressShipmentPartialView()
        {
            var command = Session["Command"];
            MySessionCartModel myCart = null;
            decimal optionShipmentPrice = 0;
            

            if (command != null)
            {
                myCart = (MySessionCartModel)command;
                optionShipmentPrice = myCart.OptionShipmentPrice;
            }


            var model = new CustomerAccountViewModel
            {
                CustomerUser_FK = CurrentUser.User?.User_ID,
                Address_FK = CurrentUser.User?.Customer_User?.CustomerAddressInvoice?.Address?.Address_ID,
                OptionShipmentPrice = optionShipmentPrice
            };
            return PartialView("~/Views/Command/CommandAddressShipment.cshtml", model);
        }

        //[CustomerAuthorize]
        [AjaxOrChildActionOnly]
        public ActionResult CommandCheckoutPartialView()
        {
            #region old code
            //var command = Session["Command"];
            //MySessionCartModel myCart = null;
            //string buttonHtml = "";

            //if (command != null)
            //{
            //    myCart = (MySessionCartModel)command;
            //    if (myCart.commandProducts.Count() > 0)
            //    {
            //        var paypalApi = new PaypalAPI();
            //        buttonHtml = paypalApi.createPaymentButton(myCart);
            //    }
            //}

            //var checkoutViewModel = new CheckoutViewModel
            //{
            //    paymentButtonHtml = new HtmlString(buttonHtml).ToHtmlString()
            //};

            //return PartialView("~/Views/Command/CommandCheckout.cshtml", checkoutViewModel);
            #endregion

            return PartialView("~/Views/Command/CommandCheckout.cshtml");
        }
        #endregion


        [AjaxOrChildActionOnly]
        //[CustomerAuthorize]
        [HttpPost]
        public ActionResult ProcessCheckout(CheckoutPaymentForm form)
        {
            var command = Session["Command"];
            MySessionCartModel myCart = null;
            string buttonHtml = "";

            if (command != null)
            {
                myCart = (MySessionCartModel)command;
                if (myCart.commandProducts.Count() > 0)
                {
                    var commandEntity = command_Repository.createCommand(myCart, form);
                    command_Repository.Save();

                    var paypalApi = new PaypalAPI();
                    var html = paypalApi.createPaymentButton(commandEntity);
                    html = html.Replace("form ", "form class=\"hiddenForm\" id=\"checkoutPaypalForm\" ");
                    //html = html.Replace("form ", "form id=\"checkoutPaypalForm\" ");
                    buttonHtml = html;

                    //Session.Remove("Command"); // clear cart session
                    myCart.resetCart(doRechargeStockQuantity: false);
                }
            }

            command_Repository.Save();

            return Json(new
            {
                success = true,
                buttonHtml = buttonHtml
            });
        }

        [AjaxOrChildActionOnly]
        [CustomerAuthorize]
        [HttpPost]
        public ActionResult computerDeliveryCharges(bool isOptionShipmentChecked)
        {
            var command = Session["Command"];
            MySessionCartModel myCart = null;
            decimal deliveryCharges = 0;
            decimal totalCommand = 0;

            if (command != null)
            {
                myCart = (MySessionCartModel)command;
                deliveryCharges = myCart.DeliveryCharge;
                totalCommand = (decimal)myCart.TotalCommandWithDeliveryCharge;
            }

            if (isOptionShipmentChecked)
            {
                deliveryCharges += myCart.OptionShipmentPrice;
                totalCommand += myCart.OptionShipmentPrice;
            }

            return Json(new
            {
                success = true,
                optionShipmentPrice = deliveryCharges,
                totalCommand = totalCommand
            });
        }

        //[AllowAnonymous]
        //[HttpPost]
        [HttpGet]
        public ActionResult checkAwaitingPaymentCommand(string email)
        {

            //if (email == null)
            //{
            //    return Json(new
            //    {
            //        HttpStatusCode = HttpStatusCode.BadRequest,
            //        message = "checkAwaitingCommand: Il manque le paramètre email"
            //    }, JsonRequestBehavior.AllowGet);
            //}

            //email = email.Trim();

            //if (!Util.IsValidEmail(email))
            //{
            //    return Json(new
            //    {
            //        HttpStatusCode = HttpStatusCode.BadRequest,
            //        message = "checkAwaitingCommand: Le mail n'est pas valide"
            //    }, JsonRequestBehavior.AllowGet);
            //}

            try
            {
                var datetimeNow = DateTime.Now;
                int minutesTimeLimit;

                var isParseOk = Int32.TryParse(PaypalConfig.PaypalConfigDependingOnMode["ExpiredLimitMinutesTime"], out minutesTimeLimit);
                if (!isParseOk)
                {
                    minutesTimeLimit = 60;
                }

                Func<DateTime?, bool> isCommandExpired = (createDateTime) =>
                {
                    if (createDateTime == null)
                    {
                        return false;
                    }
                    var datetimeCasted = (DateTime)createDateTime;
                    var isMinutesTimeLimitExpired = (datetimeNow - datetimeCasted).TotalMinutes >= minutesTimeLimit;

                    return isMinutesTimeLimitExpired;
                };

                var commandExpired_Set = command_Repository.FindAllCommand().Where(item => item.Command_Status == Command_Status.Command_AwaitingPayment)
                                                              .AsEnumerable()
                                                              .Where(item => isCommandExpired(item.CreationDateTime));
                var count = commandExpired_Set.Count();

                foreach (var item in commandExpired_Set)
                {
                    item.rechargeStockQuantity();
                    item.Command_Status = Command_Status.Command_Expired;
                    item.addCommand_History(Command_History_Action.Command_Expired, "La Commande a dépassée la limite (" + minutesTimeLimit + " minute(s)) pour le paiement Paypal, et a donc été mis à l'état Expirée.");
                    item.addCommand_History(Command_History_Action.Command_Expired, "La Commande a été déchargée vers les stocks");
                }

                if (commandExpired_Set.Count() >= 1)
                {
                    command_Repository.Save();
                }

                if (email != null && Util.IsValidEmail(email.Trim()))
                {
                    email = email.Trim();
                    Util.sendEmail("checkAwaitingPaymentCommand: SUCCESS : Nous avons bien exécuté la méthode", "checkAwaitingPaymentCommandTest", (string)email);
                }


                return Json(new
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    message = "checkAwaitingCommand: " + count + " commande(s) expirée(s) car la limite (" + minutesTimeLimit + " minute(s))  de paiement Paypal a été dépassée."
                }, JsonRequestBehavior.AllowGet);

                //Reply back a 200 code
                //return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                if (email != null && Util.IsValidEmail(email.Trim()))
                {
                    email = email.Trim();
                    Util.sendEmail("checkAwaitingPaymentCommand: ERROR : Il y a eu une erreur:" + e.InnerException, "checkAwaitingPaymentCommandTest", (string)email);
                }

                return Json(new
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    message = "checkAwaitingCommand: InnerException: " + e.InnerException
                }, JsonRequestBehavior.AllowGet);

                //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        private decimal computeDeliveryChargeForCart(MySessionCartModel myCart)
        {
            decimal priceDecimal = 0;
            if (myCart.commandProducts.Count() == 0)
            {
                return priceDecimal;
            }

            var totalWeightCart = myCart.TotalCartPackagedWeight;
            if (totalWeightCart == 0)
            {
                totalWeightCart = 1;
            }

            #region Old Code using LaPoste API with Rest method "TarifEnvoi"
            //var result = new LaPosteAPI().TarifEnvoi(totalWeightCart, TypeEnvoi.colis);
            //if (!result.isSuccess())
            //{
            //    throw new Exception("Error during LaPosteAPI for getting TarifEnvoi");
            //}

            //var itemBureau = result.Tarifs_Set.Where(item => item.channel.Equals("bureau")).FirstOrDefault();
            //if (itemBureau == null)
            //{
            //    throw new Exception("TarifEnvoi does not contain expected response");
            //} 

            //var isParseOk = Decimal.TryParse(, out priceDecimal);
            //var isParseOk = Decimal.TryParse(itemBureau.price, NumberStyles.Any, new CultureInfo("en-US"), out priceDecimal);
            #endregion

            priceDecimal = laPoste_Repository.FindColissimoCostValueByWeight(totalWeightCart);



            return priceDecimal;
        }

        //public ActionResult testCommandMail()
        //{
        //    var command = command_Repository.FindCommand(140);
        //    command.sendEmailPaymentConfirmationToUser("http://localhost:54392");
        //    command.sendEmailPaymentConfirmationForAdministrators("http://localhost:54392");

        //    return null;
        //}
    }
}
