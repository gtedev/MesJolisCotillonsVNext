using MesJolisCotillons.Extensions.XmlData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.Models
{
    public class Product_XmlData : XmlSerializer<Product_XmlData>
    {
        public decimal Height { get; set; } = 0;
        public decimal Width { get; set; } = 0;
        public decimal Depth { get; set; } = 0;
        public decimal Diameter { get; set; } = 0; 
        public int Weight { get; set; } = 0;

        public ProductType ProductType { get; set; }
        public ProductFragility ProductFragility { get; set; }
        public ProductDeliveryBoxType ProductDeliveryBoxType { get; set; }

        public int ProductPackagedWeight
        {
            get
            {
                //return this.Weight + FragilityWeight + DeliveryBoxWeight;
                return this.Weight + FragilityWeight;
            }
        }

        public int FragilityWeight
        {
            get
            {
                var result = 0;

                switch (ProductFragility)
                {
                    case ProductFragility.LEVEL1:
                        result = 50;
                        break;
                    case ProductFragility.LEVEL2:
                        result = 100;
                        break;
                    case ProductFragility.LEVEL3:
                        result = 150;
                        break;
                }

                return result;
            }
        }

        public int DeliveryBoxWeight
        {
            get
            {
                var result = 0;

                switch (ProductDeliveryBoxType)
                {
                    case ProductDeliveryBoxType.SMALL:
                        result = 100;
                        break;
                    case ProductDeliveryBoxType.MEDIUM:
                        result = 250;
                        break;
                    case ProductDeliveryBoxType.LARGE:
                        result = 400;
                        break;
                }

                return result;
            }
        }
    }

    public enum ProductType : int
    {
        UNIQUE = 1,
        SERIE = 2,
        UNDEFINED = 0
    }

    public enum ProductFragility : int
    {
        LEVEL1 = 1,
        LEVEL2 = 2,
        LEVEL3 = 3,
        UNDEFINED = 0
    }
    public enum ProductDeliveryBoxType : int
    {
        SMALL = 1,
        MEDIUM = 2,
        LARGE = 3,
        UNDEFINED = 0
    }
}

