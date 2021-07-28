using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class Command
    {
        public Command()
        {
            CommandAddress = new HashSet<CommandAddress>();
            CommandDocument = new HashSet<CommandDocument>();
            CommandHistory = new HashSet<CommandHistory>();
            CommandProduct = new HashSet<CommandProduct>();
        }

        public int CommandId { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public int CommandStatus { get; set; }
        public int CustomerUserFk { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public decimal? OptionShipmentCharge { get; set; }
        public int? OptionShipmentSecureType { get; set; }
        public int? ShipmentType { get; set; }
        public string XmlData { get; set; }

        public CustomerUser CustomerUserFkNavigation { get; set; }
        public ICollection<CommandAddress> CommandAddress { get; set; }
        public ICollection<CommandDocument> CommandDocument { get; set; }
        public ICollection<CommandHistory> CommandHistory { get; set; }
        public ICollection<CommandProduct> CommandProduct { get; set; }
    }
}
