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
    
    public enum Command_Status : int
    {
        Command_Disabled = -100,
        Command_Created = 0,
        Command_AwaitingPayment = 100,
        Command_Paid = 200,
        Command_Delivered = 300,
        Command_ReceptionConfirmed = 400,
        Command_Expired = -200,
        Command_Declined = -300
    }
}