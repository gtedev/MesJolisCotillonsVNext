using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using static MesJolisCotillons.Area._Admin.Controllers.Direct.CommandDirectController;

public static class EnumTranslation
{
    public static string ToStringFrench(this Command_Status enumValue)
    {
        var result = "";
        switch (enumValue)
        {
            case Command_Status.Command_Created:
                result = "Créé";
                break;
            case Command_Status.Command_AwaitingPayment:
                result = "En attente de paiement";
                break;
            case Command_Status.Command_Paid:
                result = "Payée";
                break;
            case Command_Status.Command_Delivered:
                result = "Colis envoyé";
                break;
            case Command_Status.Command_Disabled:
                result = "Désactivée";
                break;
            case Command_Status.Command_Expired:
                result = "Expirée";
                break;
            case Command_Status.Command_Declined:
                result = "Déclinée";
                break;
        }

        return result;
    }

    public static string ToStringFrench(this CommandActionRadioForcePayment enumValue)
    {
        var result = "";
        switch (enumValue)
        {
            case CommandActionRadioForcePayment.Cheque:
                result = "Chèque";
                break;
            case CommandActionRadioForcePayment.TransferPayment:
                result = "Virement bancaire";
                break;
            case CommandActionRadioForcePayment.Cash:
                result = "Espèce";
                break;
            case CommandActionRadioForcePayment.PaypalOutOfWebSite:
                result = "Paypal, hors site";
                break;
            case CommandActionRadioForcePayment.Other:
                result = "Autre";
                break;
        }

        return result;
    }

    public static string ToStringFrench(this ShipmentType enumValue)
    {
        var result = "";
        switch (enumValue)
        {
            case ShipmentType.NormalShipment:
                result = "Normal";
                break;
            case ShipmentType.OptionSecureShipment:
                result = "Option sécurité";
                break;
            case ShipmentType.NoShipment:
                result = "Pas de livraison";
                break;
        }

        return result;
    }

    public static string ToStringFrench(this Command_History_Action enumValue)
    {
        var result = "";
        switch (enumValue)
        {
            case Command_History_Action.Command_Created:
                result = "Commande créé";
                break;
            case Command_History_Action.Command_AwaitingPayment:
                result = "Commande en Attente de paiement";
                break;
            case Command_History_Action.Paypal_IPN_Notification_PaymentSuccess:
                result = "Paypal Notification Succès";
                break;
            case Command_History_Action.Paypal_IPN_Notification_PaymentFailed:
                result = "Paypal Notification Echec";
                break;
            case Command_History_Action.Command_Delivered:
                result = "Commande livrée";
                break;
            case Command_History_Action.Command_Paid:
                result = "Commande payée";
                break;
            case Command_History_Action.CommandForceToStatusPaid:
                result = "Commande forcée au statut payée";
                break;
            case Command_History_Action.Command_Disabled:
                result = "Commande Désactivé";
                break;
            case Command_History_Action.Command_ReEnabled:
                result = "Commande RéActivé";
                break;
            case Command_History_Action.Paypal_IPN_Notification_PaymentDeclined:
                result = "Paypal Notification Déclinée";
                break;
            case Command_History_Action.Paypal_IPN_Notification_PaymentExpired:
                result = "Paypal Notification Expiré";
                break;
            case Command_History_Action.Command_Expired:
                result = "Paypal Notification Succès";
                break;
            case Command_History_Action.Command_Declined:
                result = "Commande déclinée";
                break;
            case Command_History_Action.Paypal_CreationPaymentButton:
                result = "Paypal Button créé";
                break;
            case Command_History_Action.CommandDeliveredPersonally:
                result = "Commande livrée personellement";
                break;
        }

        return result;
    }

}
