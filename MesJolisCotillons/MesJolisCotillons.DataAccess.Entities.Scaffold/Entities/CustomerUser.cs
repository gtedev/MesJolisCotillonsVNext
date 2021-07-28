using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class CustomerUser
    {
        public CustomerUser()
        {
            Command = new HashSet<Command>();
            CustomerAddress = new HashSet<CustomerAddress>();
        }

        public int UserFk { get; set; }
        public int Status { get; set; }
        public string XmlData { get; set; }

        public User UserFkNavigation { get; set; }
        public ICollection<Command> Command { get; set; }
        public ICollection<CustomerAddress> CustomerAddress { get; set; }
    }
}
