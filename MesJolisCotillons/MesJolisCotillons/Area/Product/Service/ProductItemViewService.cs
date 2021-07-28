using MesJolisCotillons.Extensions;
using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using System.Linq;

namespace MesJolisCotillons.Service
{
    public class ProductItemViewService : IProductItemViewService
    {
        private IProductRepository productRepository;

        public ProductItemViewService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ProductItemViewModel GetProductViewResponse(int productId, string baseUrl, string absoluteUri)
        {
            var product = productRepository.FindProduct(productId, Product_Status.Enabled);
            if (product == null)
            {
                return null;
            }

            var viewModel = new ProductItemViewModel(product.Product_ID, product.DisplayName, product.Description, product.Price)
            {
                BaseUrl = baseUrl,
                ProductItemUrl = absoluteUri,
                LinkedCategory_Set = product.Category_Set.ToList(),
                BlobSteamBase64_Set = product.GetProductStreamInBase64Images(),
                AddCartButtonView = this.GetAddCartButtonView(product.ProductStockFlag),
                ProductQuantityInfoView = this.GetProductQuantityInfoView(product.ProductStockFlag),
                Product = product  // TO DO: remove that reference
            };

            return viewModel;
        }

        public AddCartButtonViewModel GetAddCartButtonView(ProductStockFlag productStockFlag)
        {
            var addButtonCls = "active";

            switch (productStockFlag)
            {
                case ProductStockFlag.OutOfStock:
                case ProductStockFlag.UniqueOutOfStock:
                    addButtonCls = "disabled";
                    break;
            }

            return new AddCartButtonViewModel
            {
                AddButtonCls = addButtonCls
            };
        }

        public ProductQuantityInfoViewModel GetProductQuantityInfoView(ProductStockFlag productStockFlag)
        {
            var ProductQuantiteFlagCls = string.Empty;
            var ProductQuantiteInfoText = string.Empty;

            switch (productStockFlag)
            {
                case ProductStockFlag.OutOfStock:
                    ProductQuantiteFlagCls = "productOutOfStockFlagColor";
                    ProductQuantiteInfoText = "Produit épuisé";
                    break;
                case ProductStockFlag.UniqueOutOfStock:
                    ProductQuantiteFlagCls = "productUniqueOutOfStockFlagColor";
                    ProductQuantiteInfoText = "Produit vendu";
                    break;
                case ProductStockFlag.Unique:
                    ProductQuantiteFlagCls = "productUniqueFlagColor";
                    ProductQuantiteInfoText = "Produit unique";

                    break;
                case ProductStockFlag.Few:
                    ProductQuantiteFlagCls = "productFewFlagColor";
                    ProductQuantiteInfoText = "Produit limitée";
                    break;
            }

            return new ProductQuantityInfoViewModel
            {
                FlagCls = ProductQuantiteFlagCls,
                InfoText = ProductQuantiteInfoText
            };

        }
    }
}
