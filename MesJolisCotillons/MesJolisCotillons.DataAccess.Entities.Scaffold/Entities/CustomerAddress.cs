using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class CustomerAddress
    {
        public int CustomerUserFk { get; set; }
        public int AddressFk { get; set; }
        public int? Type { get; set; }

        public Address AddressFkNavigation { get; set; }
        public CustomerUser CustomerUserFkNavigation { get; set; }
    }
}
