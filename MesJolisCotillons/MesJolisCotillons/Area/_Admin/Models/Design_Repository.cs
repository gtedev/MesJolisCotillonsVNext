using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MesJolisCotillons.Area._Admin;

namespace MesJolisCotillons.Models
{
    public class Design_Repository : Repository
    {
        public IQueryable<Design_Config> FindAllDesign_Config()
        {
            return db.Design_Config;
        }

        public Design_Config getActiveDesignConfig()
        {
            return this.db.Design_Config.Where(item => item.Status == Design_Config_Status.Active)
                                        .FirstOrDefault();
        }

        public DesignConfig_XmlData getActiveDesignConfigXmlFromSession(HttpSessionStateBase currentSession)
        {
            //DesignConfig_XmlData activeDesinConfig = currentSession["ActiveDesignXmlConfig"] as DesignConfig_XmlData;

            //if (activeDesinConfig == null)
            //{

            //    activeDesinConfig = this.db.Design_Config.Where(item => item.Status == Design_Config_Status.Active)
            //                                .FirstOrDefault().Design_XmlConfig;
            //    currentSession["ActiveDesignXmlConfig"] = activeDesinConfig;
            //};
            var activeDesinConfig = this.db.Design_Config.Where(item => item.Status == Design_Config_Status.Active)
                                            .FirstOrDefault().Design_XmlConfig;
            return activeDesinConfig;
        }


        public void createDesignConfig(string name, string description = null)
        {
            var entity = new Design_Config
            {
                Name = name,
                CreationDatetime = DateTime.Now,
                Status = Design_Config_Status.Creation,
                Description = description
            };
            this.db.Design_Config.Add(entity);
        }

        public void deleteDesignCOnfig(Design_Config designConfig)
        {
            this.db.Design_Config.Remove(designConfig);
        }

        public void addEtiquette(Etiquette etiquette)
        {
            this.db.Etiquettes.Add(etiquette);
        }

        public Etiquette FindEtiquetteByName(string Etiquette_Name)
        {
            return this.db.Etiquettes.Where(item => item.Etiquette_Name == Etiquette_Name)
                                     .FirstOrDefault();
        }

        public Etiquette FindEtiquette(int Etiquette_ID)
        {
            return this.db.Etiquettes.Where(item => item.Etiquette_ID == Etiquette_ID)
                                     .FirstOrDefault();
        }

        public IQueryable<Etiquette> FindAllEtiquette()
        {
            return this.db.Etiquettes;
        }

        public void deleteEtiquette(Etiquette etiquette)
        {
            this.db.Etiquettes.Remove(etiquette);
        }

        public CarouselImage FindCarouselImage(int CarouselImage_ID)
        {
            return this.db.CarouselImages.Where(item => item.CarouselImage_ID == CarouselImage_ID)
                                     .FirstOrDefault();
        }
        public CarouselImage FindCarouselImageByName(string CarouselImage_Name)
        {
            return this.db.CarouselImages.Where(item => item.CarouselImage_Name == CarouselImage_Name)
                                     .FirstOrDefault();
        }

        public void addCarouselImage(CarouselImage carouselImage)
        {
            this.db.CarouselImages.Add(carouselImage);
        }
        public void deleteCarouselImage(CarouselImage carouselImage)
        {
            this.db.CarouselImages.Remove(carouselImage);
        }
        public IQueryable<CarouselImage> FindAllCarouselImage()
        {
            return this.db.CarouselImages;
        }
    }
}