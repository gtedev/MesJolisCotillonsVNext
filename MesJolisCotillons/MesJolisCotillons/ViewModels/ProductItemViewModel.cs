using MesJolisCotillons.Models;
using System.Collections.Generic;

namespace MesJolisCotillons.ViewModels
{
    public class ProductItemViewModel
    {
        public int Product_ID { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public decimal? Price { get; private set; }

        public ProductItemViewModel(int productId, string displayName, string description, decimal? price)
        {
            this.Product_ID = productId;
            this.DisplayName = displayName;
            this.Description = description;
            this.Price = price;
        }

        //TO DO: Remove the reference to Product
        public Product Product { get; set; }

        public string ProductItemUrl { get; set; }
        public string BaseUrl { get; set; }
        public List<Category> LinkedCategory_Set { get; set; }
        public IEnumerable<string> BlobSteamBase64_Set { get; set; }

        public AddCartButtonViewModel AddCartButtonView { get; set; }
        public ProductQuantityInfoViewModel ProductQuantityInfoView { get; set; }

        public FacebookSharingParametersViewModel FacebookSharingParameters
        {
            get
            {
                return new FacebookSharingParametersViewModel
                {
                    metaLocalProperty = "fr_FR",
                    metaTypeProperty = "article",
                    metaTitleProperty = this.DisplayName,
                    metaDescriptionProperty = this.Description,
                    metaImageProperty = BaseUrl + "/Product/GetFirstProductImage/?fileName=" + this.Product_ID + ".jpg",
                    metaUrlProperty = BaseUrl + "/Product/GetProductView/" + this.Product_ID
                };
            }
        }
    }
}