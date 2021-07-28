using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class ProductCategory
    {
        public int ProductFk { get; set; }
        public int CategoryFk { get; set; }

        public Category CategoryFkNavigation { get; set; }
        public Product ProductFkNavigation { get; set; }
    }
}
