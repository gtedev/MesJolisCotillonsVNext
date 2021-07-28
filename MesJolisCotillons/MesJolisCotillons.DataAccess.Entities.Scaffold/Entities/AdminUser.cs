using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class AdminUser
    {
        public int UserFk { get; set; }

        public User UserFkNavigation { get; set; }
    }
}
