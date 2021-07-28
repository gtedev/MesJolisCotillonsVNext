using System.Collections.Generic;
using System.Linq;
using MesJolisCotillons.Models;

namespace MesJolisCotillons.ViewModels
{
    public class MySessionCartModel
    {
        private Command cmd;


        public List<CommandProductModel> commandProducts { get; set; } = new List<CommandProductModel>();
        public decimal? TotalCommand
        {
            get
            {
                decimal? total = 0;
                if (this?.commandProducts?.Count > 0)
                {
                    total = this.commandProducts?
                            .Select(item => item.TotalCommandProduct)
                            .Aggregate((a, b) => a + b);
                }

                return total;
            }
        }

        public decimal? TotalCommandWithDeliveryCharge
        {
            get
            {
                return TotalCommand + DeliveryCharge;
            }
        }
        public int TotalProductCount
        {
            get
            {
                var result = 0;
                if (this.commandProducts.Count > 0)
                {
                    result = this.commandProducts.Select(item => item.quantity).Aggregate((a, b) => a + b);
                }

                return result;
            }
        }

        public decimal OptionShipmentPrice
        {
            get
            {
                decimal result = 0;

                if (OptionShipmentSecureType == null)
                    return result;

                switch (OptionShipmentSecureType)
                {
                    case OptionShipmentType.Option1:
                        result = 3.5m;
                        break;
                    case OptionShipmentType.Option2:
                        result = 4;
                        break;
                    case OptionShipmentType.Option3:
                        result = 5.1m;
                        break;
                }


                return result;
            }
        }

        public int TotalCartPackagedWeight
        {
            get
            {
                return commandProducts.Sum(item => item.product.ProductPackagedWeight);
            }
        }
        public OptionShipmentType? OptionShipmentSecureType
        {
            get
            {
                OptionShipmentType? result = null;

                if (TotalProductCount == 0)
                    return null;

                var totalCommand = (decimal)TotalCommand;
                if (totalCommand <= 31)
                {
                    result = OptionShipmentType.Option1;
                }
                else if (totalCommand > 31 && totalCommand < 153)
                {
                    result = OptionShipmentType.Option1;
                }
                else if (totalCommand > 153)
                {
                    result = OptionShipmentType.Option3;
                }

                return result;
            }
        }

        public void resetCart(bool doRechargeStockQuantity = true)
        {
            var productRepository = new Product_Repository();
            DeliveryCharge = 0;

            foreach (var item in commandProducts.ToList())
            {
                if (doRechargeStockQuantity)
                {
                    var productEntity = productRepository.FindProduct(item.product.Product_ID);
                    if (productEntity != null)
                    {
                        //productEntity.StockQuantity += item.quantity;
                        if (productEntity.Product_XmlData.ProductType == ProductType.SERIE)
                        {
                            productEntity.StockQuantity += item.quantity;
                        }
                        else if (productEntity.Product_XmlData.ProductType == ProductType.UNIQUE)
                        {
                            productEntity.StockQuantity = 1;
                        }
                    }
                }
                commandProducts.Remove(item);
            }

            if (doRechargeStockQuantity)
            {
                productRepository.Save();
            }
        }

        //public decimal DeliveryCharge { get; set; }
        #region old DeliveryCharge
        private decimal _deliveryCharge = 0;
        public decimal DeliveryCharge
        {
            get
            {
                if (commandProducts.Count() == 0)
                {
                    return 0;
                }
                return _deliveryCharge;
            }
            set { _deliveryCharge = value; }

        }
        #endregion
    }

    public class CommandProductModel
    {
        public int quantity { get; set; }
        public ProductItemModel product { get; set; }
        decimal TotalCommandProductPrice { get; set; }
        public decimal? TotalCommandProduct
        {
            get
            {
                return this.product.Price * this.quantity;
            }
        }
    }
}