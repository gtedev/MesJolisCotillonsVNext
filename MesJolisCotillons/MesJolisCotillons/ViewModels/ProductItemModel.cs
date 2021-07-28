using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.ViewModels
{
    public class ProductItemModel
    {
        public int Product_ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<int> BlobsIds { get; set; }
        public decimal Price { get; set; }
        public int ProductPackagedWeight { get; set; }

        //public HtmlString AddButtonHtml { get; set; }
        //public HtmlString ProductQuantiteInfoHtml { get; set; } 
        public string AddButtonHtml { get; set; }
        public string ProductQuantiteInfoHtml { get; set; }
        public ProductType ProductType { get; set; }
        public int MaxQuantity { get; set; }
    }
}