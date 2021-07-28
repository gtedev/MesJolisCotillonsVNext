using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class Product
    {
        public Product()
        {
            CommandProduct = new HashSet<CommandProduct>();
            ProductBlob = new HashSet<ProductBlob>();
            ProductCategory = new HashSet<ProductCategory>();
            ProductKeyWord = new HashSet<ProductKeyWord>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int Status { get; set; }
        public int? StockQuantity { get; set; }
        public string DisplayName { get; set; }
        public int? MainBlobFk { get; set; }
        public int? ProductFlags { get; set; }
        public string XmlData { get; set; }

        public Blob MainBlobFkNavigation { get; set; }
        public ICollection<CommandProduct> CommandProduct { get; set; }
        public ICollection<ProductBlob> ProductBlob { get; set; }
        public ICollection<ProductCategory> ProductCategory { get; set; }
        public ICollection<ProductKeyWord> ProductKeyWord { get; set; }
    }
}
