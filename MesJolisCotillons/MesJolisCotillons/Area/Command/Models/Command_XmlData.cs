using MesJolisCotillons.Extensions.Paypal;
using MesJolisCotillons.Extensions.XmlData;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MesJolisCotillons.Area._Admin.Controllers.Direct.CommandDirectController;
using static MesJolisCotillons.Area.PaypalNotification.Controllers.IPNController;

namespace MesJolisCotillons.Models
{
    public class Command_History_XmlData : XmlSerializer<Command_History_XmlData>
    {
        public string Description { get; set; }
        public string CommentFromAdminUser { get; set; }

        public DateTime? ShipmentDate { get; set; }
        public decimal? FeesPaidInPostOffice { get; set; }
        public CommandActionRadioForcePayment? ForcePaymentReason { get; set; }
        public string ColissimoNumber { get; set; }
        public string Paypal_Txn_Id { get; set; }
        public PaypalCreatedButtonInfos paypalButtonInfos { get; set; }

        public bool IsPaypalCreationButtonAliveHosted
        {
            get
            {
                return paypalButtonInfos?.ButtonCode == ButtonCodeType.HOSTED && paypalButtonInfos?.Status == PaypalHostedButtonStatus.HOSTED;
            }
        }

        public string PaypalHostedButtonID
        {
            get
            {
                return paypalButtonInfos?.HostedButtonID;
            }
        }
    }
    public class CommandProduct_XmlData : XmlSerializer<CommandProduct_XmlData>
    {
        public int StockQuantity { get; set; }
    }
    public class Command_XmlData : XmlSerializer<Command_XmlData>
    {
        public Command_History_Action? CommandChoiceAction { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public decimal? FeesPaidInPostOffice { get; set; }
        public string ColissimoNumber { get; set; }
        public string CommentFromAdminUser { get; set; }
        public PaypalCreatedButtonInfos paypalButtonInfos { get; set; }
        public PaypalNotificationForm PaypalNotificationForm { get; set; } 
    }
    public class PaypalCreatedButtonInfos
    {
        public ButtonTypeType ButtonType { get; set; }
        public ButtonCodeType ButtonCode { get; set; }
        public string HostedButtonID { get; set; }
        public PaypalHostedButtonStatus? Status { get; set; } = null;
    }
}