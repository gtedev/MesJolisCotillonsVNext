using MesJolisCotillons.Area._Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.ViewModels
{
    public class HeaderViewModel
    {
        public DesignConfig_XmlData DesignConfigXmlData { get; set; }
        public string CustomerUserName { get; set; }
        public int TotalProductCountCart { get; set; }
        public decimal TotalPriceCart { get; set; }
        public bool isCustomerUserConnected { get; set; }
    }
}