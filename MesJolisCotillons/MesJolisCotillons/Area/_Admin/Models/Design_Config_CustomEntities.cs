using MesJolisCotillons.Area._Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.Models
{
    public partial class Design_Config
    {
        private DesignConfig_XmlData _designXmlConfig = null;
        public DesignConfig_XmlData Design_XmlConfig
        {
            get
            {
                if (_designXmlConfig == null)
                {
                    _designXmlConfig = DesignConfig_XmlData.Deserialize<DesignConfig_XmlData>(this.xmlData);
                }

                return _designXmlConfig;
            }
        }

        public void updateMenuItemProperty(string menuItemId, string propertyName, string value, bool? checkedValue)
        {
            var designXmlConfig = this.Design_XmlConfig;

            var menuItem = designXmlConfig.FindHeaderMenuItem(menuItemId);
            menuItem.updateActionProperty(propertyName, value, checkedValue);
            this.xmlData = designXmlConfig.Serialize();
            this.LastUpdateDatetime = new DateTime?(DateTime.Now);
        }

        public MenuItem FindHeaderMenuItem(string menuItemId)
        {
            var designXmlData = this.Design_XmlConfig;
            return designXmlData.FindHeaderMenuItem(menuItemId);
        }

        public Design_Config.CurrentUserAbilityModel CurrentUserAbility
        {
            get
            {
                return new CurrentUserAbilityModel
                {
                    canEditDesignConfig = true,
                    canActiveDesignConfig = this.Status != Design_Config_Status.Active,
                    canDeleteDesignConfig = this.Status != Design_Config_Status.Active
                };
            }
        }

        public void RemoveMenuItem(string itemId)
        {
            var designXmlData = this.Design_XmlConfig;

            designXmlData.RemoveMenuItem(itemId);
            this.xmlData = designXmlData.Serialize();
            this.LastUpdateDatetime = new DateTime?(DateTime.Now);
        }


        public void AddMenuItem(string targetItemId, string itemTypeToAdd, string targetItemType)
        {
            MenuItem menuItem = null;
            var designXmlData = this.Design_XmlConfig;

            if (typeof(OneGridProductMenuItem).Name == itemTypeToAdd)
            {
                menuItem = (MenuItem)new OneGridProductMenuItem();
            }

            if (typeof(UrlMenuItem).Name == itemTypeToAdd)
            {
                menuItem = (MenuItem)new UrlMenuItem();
            }

            if (typeof(MenuItem).Name == targetItemType)
            {
                var headerMenuItem = designXmlData.FindHeaderMenuItem(targetItemId);
                headerMenuItem.AddSubMenuItem(menuItem);
            }

            else if ("HeaderRoot" == targetItemType)
            {
                designXmlData.AddMenuItemToHeader(menuItem);
            }

            this.xmlData = this.Design_XmlConfig.Serialize();
            this.LastUpdateDatetime = new DateTime?(DateTime.Now);
        }

        public void AddCarouselImage(int itemId)
        {
            var designXmlData = this.Design_XmlConfig;
            var designRepository = new Design_Repository();


            var carouselImage = designRepository.FindCarouselImage(itemId);
            if (carouselImage != null)
            {
                if (this.Design_XmlConfig.HomePage == null)
                {
                    this.Design_XmlConfig.HomePage = new HomePageConfig { CarousselImage_ID_Set = new List<int>() };
                }
                this.Design_XmlConfig.HomePage.CarousselImage_ID_Set.Add(itemId);
            }
            this.xmlData = this.Design_XmlConfig.Serialize();
            this.LastUpdateDatetime = new DateTime?(DateTime.Now);

        }
        public void RemoveCarouselImage(string itemId, int positionItem)
        {
            var designXmlData = this.Design_XmlConfig;
            int carouselId;
            var isParseOk = Int32.TryParse(itemId, out carouselId);

            if (!isParseOk)
            {
                return;
            }

            designXmlData.RemoveCarouselImage(carouselId, positionItem);
            this.xmlData = designXmlData.Serialize();
            this.LastUpdateDatetime = new DateTime?(DateTime.Now);
        }
        public bool ChangeMenuItemPosition(string itemId, string targetItemType, int newIndexPosition)
        {
            bool isChangeSuccess = false;
            var designXmlData = this.Design_XmlConfig;

            if (typeof(MenuItem).Name == targetItemType)
            {
                isChangeSuccess = designXmlData.ChangeMenuItemPosition(itemId, targetItemType, newIndexPosition);
                if (isChangeSuccess)
                {
                    this.xmlData = designXmlData.Serialize();
                }

            }
            return isChangeSuccess;
        }

        public bool ChangeCarouselImagePosition(string itemId, string targetItemType, int newIndexPosition, int? PositionItem)
        {
            bool isChangeSuccess = false;
            var designXmlData = this.Design_XmlConfig;

            if (typeof(CarouselImage).Name == targetItemType)
            {
                isChangeSuccess = designXmlData.ChangeCarouselImagePosition(itemId, targetItemType, newIndexPosition, PositionItem);
                if (isChangeSuccess)
                {
                    this.xmlData = designXmlData.Serialize();
                }

            }
            return isChangeSuccess;
        }

        #region CurrentUserAbilityModel
        public class CurrentUserAbilityModel
        {
            public bool canEditDesignConfig { get; set; }

            public bool canActiveDesignConfig { get; set; }

            public bool canDeleteDesignConfig { get; set; }
        }
        #endregion
    }

    public partial class CarouselImage
    {
        public bool isUseSomewhere
        {
            get
            {

                bool isCarouselUsedInConfig = false;
                var designXmlConfig_Set = new Design_Repository().FindAllDesign_Config().AsEnumerable<Design_Config>().Select(item => item.Design_XmlConfig);

                foreach (var designConfigXmlData in designXmlConfig_Set)
                {
                    isCarouselUsedInConfig = designConfigXmlData.isCarouselImageUsedInConfig(this);
                    if (isCarouselUsedInConfig)
                    {
                        return isCarouselUsedInConfig;
                    }
                }

                return isCarouselUsedInConfig;
            }
        }
    }
}