using MesJolisCotillons.Area._Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.ViewModels
{
    public class CollapsibleMenuViewModel
    {
        public DesignConfig_XmlData DesignConfigXmlData { get; set; }
    }

    public enum CollapsibleMenuViewType : int
    {
        MenuItems = 0,
        LoginPage = 1,
        CustomerPage = 2
    }
}