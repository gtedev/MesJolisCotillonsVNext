using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public AdminUser AdminUser { get; set; }
        public CustomerUser CustomerUser { get; set; }
    }
}
