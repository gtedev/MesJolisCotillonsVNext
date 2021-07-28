using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;

namespace MesJolisCotillons.Extensions.Paypal
{
    #region PaypalAPI
    public class PaypalAPI
    {
        public string createPaymentButton(Command command)
        {
            // Create request object
            BMCreateButtonRequestType request = new BMCreateButtonRequestType();

            #region ButtonType Help
            //  (Required) The kind of button you want to create. It is one of the following values:
            //    BUYNOW - Buy Now button
            //    CART - Add to Cart button
            //    GIFTCERTIFICATE - Gift Certificate button
            //    SUBSCRIBE - Subscribe button
            //    DONATE - Donate button
            //    UNSUBSCRIBE - Unsubscribe button
            //    VIEWCART - View Cart button
            //    PAYMENTPLAN - Installment Plan button; since version 63.0
            //    AUTOBILLING - Automatic Billing button; since version 63.0
            //    PAYMENT - Pay Now button; since version 65.1
            // Note: Do not specify BUYNOW if BUTTONCODE=TOKEN; specify PAYMENT instead. 
            // Do not specify PAYMENT if BUTTONCODE=HOSTED.  
            #endregion
            var buttonType = ButtonTypeType.BUYNOW;
            request.ButtonType = buttonType;

            #region ButtonCode Help
            // (Optional) The kind of button code to create. It is one of the following values:
            // HOSTED - A secure button stored on PayPal; default for all buttons except View Cart, Unsubscribe, and Pay Now
            // ENCRYPTED - An encrypted button, not stored on PayPal; default for View Cart button
            // CLEARTEXT - An unencrypted button, not stored on PayPal; default for Unsubscribe button
            // TOKEN - A secure button, not stored on PayPal, used only to initiate the Hosted Solution checkout flow; 
            // default for Pay Now button. Since version 65.1 
            #endregion
            var creationButtonType = PaypalConfig.CreationButtonTypeCodeDepenpingOnMode;
            request.ButtonCode = creationButtonType;

            #region Add variables Help
            /* Add HTML standard button variables that control what is posted to 
          * PayPal when a user clicks on the created button. Refer the
          * "HTML Variables for Website Payments Standard" guide for more.
          */
            #endregion
            var itemDescription = command.CommandProduct_Set.AsEnumerable().Select(item => item.Quantity + " x " + item.Product.DisplayName?.Trim())
                                                           .Aggregate((a, b) => a + ", " + b);

            List<string> buttonVars = new List<string>();
            buttonVars.Add("item_name=" + itemDescription);
            buttonVars.Add("amount=" + ((decimal)command.TotalCommand).ToString(new CultureInfo("en-US")));
            if (command.ShipmentType != ShipmentType.NoShipment)
            {
                buttonVars.Add("shipping=" + ((decimal)command.DeliveryCharge).ToString(new CultureInfo("en-US")));
            }


            var facilitatorEmail = PaypalConfig.PaypalConfigDependingOnMode["facilitatorEmail"];
            var notifyUrl = PaypalConfig.PaypalConfigDependingOnMode["notifyUrl"];
            var imageUrl = PaypalConfig.PaypalConfigDependingOnMode["imageUrl"];
            var returnUrl = PaypalConfig.PaypalConfigDependingOnMode["baseUrl"];  // Paypal will display a button "Return to merchant website" that will rediret to this link

            buttonVars.Add("return=" + returnUrl);
            buttonVars.Add("business=" + facilitatorEmail);
            buttonVars.Add("notify_url=" + notifyUrl);
            buttonVars.Add("image_url=" + imageUrl);

            //buttonVars.Add("custom=" + "Command_ID:" + command.Command_ID);
            buttonVars.Add("custom=" + command.Command_ID);

            buttonVars.Add("currency_code=" + "EUR");
            buttonVars.Add("no_shipping=" + "1");  // 1- do not prompt for an address

            var commandAddressInvoice = command.CommandAddress_Set
                                               .Where(item => item.Type.HasFlag(CommanAddress_Type.Invoice))
                                               .FirstOrDefault();

            if (commandAddressInvoice != null)
            {
                var addressShipment = commandAddressInvoice.Address;
                buttonVars.Add("address1=" + addressShipment.Address1);
                buttonVars.Add("city=" + addressShipment.City);
                buttonVars.Add("zip=" + addressShipment.Zip_Code);

                buttonVars.Add("country=" + addressShipment.Country.ISO_Code); // address country
                buttonVars.Add("lc=" + addressShipment.Country.ISO_Code); //language for the billing information
            }
            buttonVars.Add("first_name=" + commandAddressInvoice.FirstName);
            buttonVars.Add("last_name=" + commandAddressInvoice.LastName);

            buttonVars.Add("email=" + command.Customer_User.User.eMail);

            request.ButtonVar = buttonVars;

            // Invoke the API 
            BMCreateButtonReq wrapper = new BMCreateButtonReq();
            wrapper.BMCreateButtonRequest = request;
            Dictionary<string, string> configurationMap = PaypalConfig.GetAcctAndConfig();

            var protocolCurrent = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Creating service wrapper object to make an API call by loading configuration map. 
            PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(configurationMap);
            BMCreateButtonResponseType response = service.BMCreateButton(wrapper);
            if (response.Ack != AckCodeType.SUCCESS)
            {
                throw new Exception("createPaymentButton: Error" + response?.Errors?.FirstOrDefault()?.ToString());
            }

            ServicePointManager.SecurityProtocol = protocolCurrent;

            var buttonResponseHtml = response.Website;
            // Check for API return status
            //setKeyResponseObjects(service, response);

            var paypalButtonInfo = new PaypalCreatedButtonInfos
            {
                ButtonCode = creationButtonType,
                ButtonType = buttonType
            };

            if (PaypalConfig.PaypalCreationButtonMode == PaypalCreationButtonMode.HOSTED)
            {
                paypalButtonInfo.HostedButtonID = response.HostedButtonID;
                paypalButtonInfo.Status = PaypalHostedButtonStatus.HOSTED;
            }

            var creationButtonMode = PaypalConfig.PaypalCreationButtonMode;
            command.addCommand_History(Command_History_Action.Paypal_CreationPaymentButton, "Boutton de transaction Paypal créé avec succès (" + creationButtonMode.ToString() + " Mode)", PaypalCreationlButtonInfo: paypalButtonInfo);

            return buttonResponseHtml;
        }

        public void deletePaymentButton(string hostedButtonID)
        {
            // Create request object
            BMManageButtonStatusRequestType request = new BMManageButtonStatusRequestType();
            request.ButtonStatus = ButtonStatusType.DELETE;
            request.HostedButtonID = hostedButtonID;

            // Invoke the API
            BMManageButtonStatusReq wrapper = new BMManageButtonStatusReq();
            wrapper.BMManageButtonStatusRequest = request;
            Dictionary<string, string> configurationMap = PaypalConfig.GetAcctAndConfig();

            var protocolCurrent = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Creating service wrapper object to make an API call by loading configuration map. 
            PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(configurationMap);
            BMManageButtonStatusResponseType response = service.BMManageButtonStatus(wrapper);

            if (response.Ack != AckCodeType.SUCCESS)
            {
                throw new Exception("deletePaymentButton: Error" + response?.Errors?.FirstOrDefault()?.ToString());
            }
        }
    }
    #endregion

    #region PaypalConfig
    public static class PaypalConfig
    {
        #region Section Web config
        private static NameValueCollection paypalModeConfig = (NameValueCollection)ConfigurationManager.GetSection("PaypalMode");
        private static NameValueCollection paypalSandboxConfig = (NameValueCollection)ConfigurationManager.GetSection("PaypalConfigSandbox");
        private static NameValueCollection paypalLiveConfig = (NameValueCollection)ConfigurationManager.GetSection("PaypalConfigLive");
        #endregion

        public static Dictionary<string, string> GetAcctAndConfig()
        {
            Dictionary<string, string> configMap = new Dictionary<string, string>();
            var configToApply = PaypalConfigDependingOnMode;

            configMap.Add("mode", PaypalModeString);

            configMap.Add("account1.apiUsername", configToApply["apiUsername"]);
            configMap.Add("account1.apiPassword", configToApply["apiPassword"]);
            configMap.Add("account1.apiSignature", configToApply["apiSignature"]);

            configMap.Add("clientId", configToApply["clientId"]);
            configMap.Add("clientSecret", configToApply["clientSecret"]);


            return configMap;
        }

        public static PaypalMode PaypalMode
        {
            get
            {
                var mode = paypalModeConfig["Mode"];
                if (mode == "sandbox")
                {
                    return PaypalMode.SANDBOX;
                }
                else
                {
                    return PaypalMode.LIVE;
                }
            }
        }
        public static PaypalCreationButtonMode PaypalCreationButtonMode
        {
            get
            {
                var mode = PaypalConfigDependingOnMode["PaypalCreationButtonMode"];
                var modeUpperCase = mode.ToUpper();

                switch (modeUpperCase)
                {
                    case "ENCRYPTED":
                        return PaypalCreationButtonMode.ENCRYPTED;

                    case "HOSTED":
                        return PaypalCreationButtonMode.HOSTED;
                    default:
                        return PaypalCreationButtonMode.HOSTED;
                }
            }
        }

        public static ButtonCodeType CreationButtonTypeCodeDepenpingOnMode
        {
            get
            {
                var creationButtmonMode = PaypalCreationButtonMode;

                switch (creationButtmonMode)
                {
                    case PaypalCreationButtonMode.ENCRYPTED:
                        return ButtonCodeType.HOSTED;
                    case PaypalCreationButtonMode.HOSTED:
                        return ButtonCodeType.HOSTED;
                    default:
                        return ButtonCodeType.HOSTED;
                }
            }
        }
        public static string PaypalModeString
        {
            get
            {
                return PaypalMode.ToString().ToLowerInvariant();
            }
        }

        public static NameValueCollection PaypalConfigDependingOnMode
        {
            get
            {
                var configResult = new NameValueCollection();
                if (PaypalMode == PaypalMode.SANDBOX)
                {
                    configResult = paypalSandboxConfig;
                }
                else if (PaypalMode == PaypalMode.LIVE)
                {
                    configResult = paypalLiveConfig;
                }

                return configResult;
            }
        }

        public static Dictionary<string, string> GetConfig()
        {
            Dictionary<string, string> configMap = new Dictionary<string, string>();

            // Endpoints are varied depending on whether sandbox OR live is chosen for mode
            configMap.Add("mode", "sandbox");

            // These values are defaulted in SDK. If you want to override default values, uncomment it and add your value.
            // configMap.Add("connectionTimeout", "5000");
            // configMap.Add("requestRetries", "2");

            return configMap;
        }
    }
    #endregion

    #region Useful class, enum
    public class CheckoutPaymentForm
    {
        public string FirstNameInvoice { get; set; }
        public string LastNameInvoice { get; set; }
        public string AddressInvoice { get; set; }
        public string CityInvoice { get; set; }
        public string ZipCodeInvoice { get; set; }
        public string CountryInvoice { get; set; }
        public int Country_FK { get; set; }

        public string FirstNameShipment { get; set; }
        public string LastNameShipment { get; set; }
        public string AddressShipment { get; set; }
        public string CityShipment { get; set; }
        public string ZipCodeShipment { get; set; }
        public string CountryShipment { get; set; }

        public bool isAddressInvoiceSameFromShipment { get; set; }
        public bool isOptionShipmentChecked { get; set; }
        public bool isNoShipmentChecked { get; set; }
        public ShipmentType? shipmentType { get; set; }
        public string emailNoLogin { get; set; } 
    }

    public enum PaypalMode : int
    {
        SANDBOX = 1,
        LIVE = 2
    }

    public enum PaypalCreationButtonMode : int
    {
        ENCRYPTED = 1,
        HOSTED = 2
    }

    public enum PaypalHostedButtonStatus : int
    {
        HOSTED = 1,
        DELETED = 2
    }
    #endregion
}