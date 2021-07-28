using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class ProductKeyWord
    {
        public int ProductFk { get; set; }
        public int KeyWordFk { get; set; }

        public KeyWord KeyWordFkNavigation { get; set; }
        public Product ProductFkNavigation { get; set; }
    }
}
