using System;
using System.Collections.Generic;
using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using System.Linq;

namespace MesJolisCotillons.Area.Cart.Service
{
    public class CartViewService : ICartViewService
    {
        private readonly IProductRepository productRepository;
        public CartViewService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public CartViewModel GetCartViewModel(MySessionCartModel cart)
        {
            this.UpdateMaxQuantityProduct(cart);
            return new CartViewModel
            {
                SessionCartModel = cart,
                ProductIdBlobs = this.GetProductIdBlobs(cart)
            };
        }

        private Dictionary<int, byte[]> GetProductIdBlobs(MySessionCartModel cart)
        {
            var productIds = cart.commandProducts.Select(p => p.product.Product_ID);
            return productRepository.FindAllProduct()
                                    .Where(p => productIds.Contains(p.Product_ID))
                                    .ToDictionary(p => p.Product_ID, p => p.Blob_Set.FirstOrDefault().Stream);

        }

        private void UpdateMaxQuantityProduct(MySessionCartModel cart)
        {
            foreach (var commandProductModel in cart.commandProducts)
            {
                var product = productRepository
                                    .FindProduct(commandProductModel.product.Product_ID);

                if (product != null)
                {
                    // maxQuantite est la somme de ce qui a ete deja reserve par le client ds son panier (commandProductModel.quantity) et ce quil reste en stock
                    var maxQuantity = product.StockQuantity == null ? 0 : (int)product.StockQuantity + commandProductModel.quantity;
                    commandProductModel.product.MaxQuantity = maxQuantity >= 10 ? 10 : maxQuantity;
                }
            }
        }
    }
}
