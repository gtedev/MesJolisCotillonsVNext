//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MesJolisCotillons.Models
{
    using System;
    
    public enum Command_History_Action : int
    {
        Command_Created = 100,
        Command_AwaitingPayment = 200,
        Paypal_IPN_Notification_PaymentSuccess = 300,
        Paypal_IPN_Notification_PaymentFailed = -100,
        Command_Delivered = 400,
        Command_Paid = 500,
        CommandForceToStatusPaid = 600,
        Command_Disabled = -200,
        Command_ReEnabled = 700,
        Paypal_IPN_Notification_PaymentDeclined = -300,
        Paypal_IPN_Notification_PaymentExpired = -400,
        Command_Expired = -500,
        Command_Declined = -600,
        Paypal_CreationPaymentButton = 120,
        CommandDeliveredPersonally = 800
    }
}
