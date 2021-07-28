using MesJolisCotillons.Area._Admin;
using MesJolisCotillons.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.Models
{
    public partial class Product
    {
        public ProductStockFlag ProductStockFlag
        {
            get
            {
                ProductStockFlag result = ProductStockFlag.ALotOfInStock;
                if (this.StockQuantity == 0 && (this.Product_XmlData.ProductType == ProductType.SERIE || this.Product_XmlData.ProductType == ProductType.UNDEFINED))
                {
                    result = ProductStockFlag.OutOfStock;
                }
                else if (this.StockQuantity == 0 && this.Product_XmlData.ProductType == ProductType.UNIQUE)
                {
                    result = ProductStockFlag.UniqueOutOfStock;
                }
                else if (this.Product_XmlData.ProductType == ProductType.UNIQUE)
                {
                    result = ProductStockFlag.Unique;
                }
                else if (this.StockQuantity >= 1 && this.StockQuantity <= 10)
                {
                    result = ProductStockFlag.Few;
                }
                else if (this.StockQuantity > 10)
                {
                    result = ProductStockFlag.ALotOfInStock;
                }

                return result;
            }
        }

        public string ProductName
        {
            get
            {
                var result = this.Name;
                if (!String.IsNullOrEmpty(this.DisplayName))
                {
                    result = this.DisplayName;
                }

                return result;
            }
        }

        public CurrentUserAbility CurrentUserAbility
        {
            get
            {
                return new CurrentUserAbility
                {
                    userCanActiveProduct = this.Status < Product_Status.Enabled,
                    userCanDeactivateProduct = this.Status == Product_Status.Enabled,
                    userCanDeleteProduct = this.CommandProduct_Set.Count() == 0 && this.Status < Product_Status.Enabled
                };
            }
        }

        private Product_XmlData _productXmldata = null;
        public Product_XmlData Product_XmlData
        {
            get
            {
                if (_productXmldata == null)
                {
                    _productXmldata = Product_XmlData.Deserialize<Product_XmlData>(this.xmlData);
                }

                return _productXmldata;
            }
        }
    }
    public class CurrentUserAbility
    {
        public bool userCanActiveProduct { get; set; }
        public bool userCanDeactivateProduct { get; set; }
        public bool userCanDeleteProduct { get; set; }

    }

    public partial class Category
    {
        public bool isUseSomewhere
        {
            get
            {
                bool hasCategoryUsed = this.Product_Set.Count() > 0;

                if (hasCategoryUsed)
                {
                    return hasCategoryUsed;
                }

                bool isCategoryUsedInConfig = false;
                var designXmlConfig_Set = new Design_Repository().FindAllDesign_Config().AsEnumerable<Design_Config>().Select(item => item.Design_XmlConfig);

                foreach (var designConfigXmlData in designXmlConfig_Set)
                {
                    isCategoryUsedInConfig = designConfigXmlData.isCategoryUsedInConfig(this);
                    if (isCategoryUsedInConfig)
                    {
                        break;
                    }
                }
                return isCategoryUsedInConfig;
            }
        }
    }
    public partial class KeyWord
    {
        public bool isUseSomewhere
        {
            get
            {
                bool hasKeyWordUsed = this.Product_Set.Count() > 0;

                if (hasKeyWordUsed)
                {
                    return hasKeyWordUsed;
                }

                bool isKeyWordUsedInConfig = false;
                var designXmlConfig_Set = new Design_Repository().FindAllDesign_Config().AsEnumerable<Design_Config>().Select(item => item.Design_XmlConfig);

                foreach (var designConfigXmlData in designXmlConfig_Set)
                {
                    isKeyWordUsedInConfig = designConfigXmlData.isKeyWordUsedInConfig(this);
                    if (isKeyWordUsedInConfig)
                    {
                        break;
                    }
                }
                return isKeyWordUsedInConfig;
            }
        }
    }

    public partial class Etiquette
    {
        public bool isUseSomewhere
        {
            get
            {
                bool hasKeyWordUsed = this.Category_Set.Count() > 0;

                if (hasKeyWordUsed)
                {
                    return hasKeyWordUsed;
                }

                bool isEtiquettteUsedInConfig = false;
                var designXmlConfig_Set = new Design_Repository().FindAllDesign_Config().AsEnumerable<Design_Config>().Select(item => item.Design_XmlConfig);

                foreach (var designConfigXmlData in designXmlConfig_Set)
                {
                    isEtiquettteUsedInConfig = designConfigXmlData.isEtiquetteUsedInConfig(this);
                    if (isEtiquettteUsedInConfig)
                    {
                        break;
                    }
                }
                return isEtiquettteUsedInConfig;
            }
        }
    }
}

public enum ProductStockFlag : int
{
    UniqueOutOfStock = -2000,
    OutOfStock = -1000,
    Unique = 0,
    Few = 100,
    ALotOfInStock = 200,
    TooMuchInStock = 400
}
