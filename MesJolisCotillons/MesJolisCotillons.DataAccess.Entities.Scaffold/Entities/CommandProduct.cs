using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class CommandProduct
    {
        public int CommandProductId { get; set; }
        public int CommandFk { get; set; }
        public int ProductFk { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public string XmlData { get; set; }
        public decimal ProductPrice { get; set; }

        public Command CommandFkNavigation { get; set; }
        public Product ProductFkNavigation { get; set; }
    }
}
