namespace MesJolisCotillons.DataAccess.Entities.EntityModels
{
    using System;
    using System.Collections.Generic;

    public class Category
    {
        public Category()
        {
            this.ProductCategories = new HashSet<ProductCategory>();
        }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public int? EtiquetteFk { get; set; }

        ////public Etiquette EtiquetteFkNavigation { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
