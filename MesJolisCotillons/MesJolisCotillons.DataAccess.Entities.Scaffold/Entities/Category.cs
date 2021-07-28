using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class Category
    {
        public Category()
        {
            ProductCategory = new HashSet<ProductCategory>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? EtiquetteFk { get; set; }

        public Etiquette EtiquetteFkNavigation { get; set; }
        public ICollection<ProductCategory> ProductCategory { get; set; }
    }
}
