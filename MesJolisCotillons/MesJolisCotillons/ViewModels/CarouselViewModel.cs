using MesJolisCotillons.Area._Admin;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.ViewModels
{
    public class CarouselViewModel
    {
        public DesignConfig_XmlData DesignConfigXmlData { get; set; }
        public List<CarouselImage> CarouselImage_Set { get; set; }
        public int WidthScreen { get; set; }

        public bool hasCarouselImagesAnyElements
        {
            get
            {
                return DesignConfigXmlData?.HomePage?.CarousselImage_ID_Set.Count > 0;
            }
        }
    }
}