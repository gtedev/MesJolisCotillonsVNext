using System.Collections.Generic;

namespace MesJolisCotillons.Contracts.ViewModels.Product
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
        }

        public ProductViewModel(
            int productId,
            string name,
            string description,
            decimal? price,
            string displayName,
            IReadOnlyCollection<string> productBase64Images)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
            DisplayName = displayName;
            ProductBase64Images = productBase64Images;
        }

        public int ProductId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal? Price { get; private set; }

        public string DisplayName { get; private set; }

        public IReadOnlyCollection<string> ProductBase64Images { get; private set; }
    }
}
