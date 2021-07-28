using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class CommandAddress
    {
        public int Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AddressFk { get; set; }
        public int CommandFk { get; set; }

        public Address AddressFkNavigation { get; set; }
        public Command CommandFkNavigation { get; set; }
    }
}
