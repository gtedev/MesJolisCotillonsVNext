using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MesJolisCotillons.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home_
        private Design_Repository designRepository = new Design_Repository();
        private Util_Repository util_Repository = new Util_Repository();

        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        #region PartialView
        //[OutputCache(Duration = 1800)]
        [AjaxOrChildActionOnly]
        public ActionResult HeaderPartialView()
        {
            var command = Session["Command"];
            MySessionCartModel myCart = null;
            int totalCountProduct = 0;
            decimal totalPriceCart = 0;
            string customerUserNameOrDefault = "se connecter";
            bool isCustomerUserConnected = false;

            if (command != null)
            {
                myCart = (MySessionCartModel)command;
                totalCountProduct = myCart.TotalProductCount;
                totalPriceCart = myCart.TotalCommand != null ? (decimal)myCart.TotalCommand : 0;
            }

            if (!String.IsNullOrEmpty(CurrentUser.User?.FullName))
            {
                customerUserNameOrDefault = CurrentUser.User?.FullName;
                isCustomerUserConnected = true;
            }


            var designConfigXmlData = designRepository.getActiveDesignConfigXmlFromSession(Session);

            var headerViewModel = new HeaderViewModel
            {
                CustomerUserName = customerUserNameOrDefault,
                DesignConfigXmlData = designConfigXmlData,
                TotalProductCountCart = totalCountProduct,
                TotalPriceCart = totalPriceCart,
                isCustomerUserConnected = isCustomerUserConnected
            };

            return PartialView("~/Views/Shared/Header.cshtml", headerViewModel);
        }

        [AjaxOrChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public ActionResult CarouselPartialView(int widthScreen)
        {
            var designConfigXmlData = designRepository.getActiveDesignConfigXmlFromSession(Session);

            var carouselViewModel = new CarouselViewModel
            {
                DesignConfigXmlData = designConfigXmlData,
                CarouselImage_Set = designConfigXmlData?.HomePage?.CarousselImage_Set,
                WidthScreen = widthScreen
            };

            return PartialView("~/Views/Home/CarouselView.cshtml", carouselViewModel);
        }

        [AjaxOrChildActionOnly]
        public ActionResult CollapsibleMenuPartialView(CollapsibleMenuViewType type = CollapsibleMenuViewType.MenuItems)
        {
            switch (type)
            {
                case CollapsibleMenuViewType.CustomerPage:
                    throw new NotImplementedException();
                case CollapsibleMenuViewType.LoginPage:
                    throw new NotImplementedException();
                case CollapsibleMenuViewType.MenuItems:
                default:
                    var designConfigXmlData = designRepository.getActiveDesignConfigXmlFromSession(Session);
                    var collapsibleMenuViewModel = new CollapsibleMenuViewModel
                    {
                        DesignConfigXmlData = designConfigXmlData
                    };

                    return PartialView("~/Views/Shared/CollapsibleMenu.cshtml", collapsibleMenuViewModel);
            }


        }
        #endregion

        [HttpGet]
        public ActionResult GetCarouselImage(Int32 id)
        {
            var designConfigXml = designRepository.getActiveDesignConfigXmlFromSession(Session);
            var carouseImage = designConfigXml.HomePage.CarousselImage_Set.ElementAtOrDefault(id);

            if (carouseImage == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Image non trouvé dans le Carousel"
                }, JsonRequestBehavior.AllowGet);
            }
            var blob = carouseImage.Blob;
            ImageResult result = new ImageResult(blob.Stream, blob.MimeType);

            return result;
        }

        [AjaxOrChildActionOnly]
        [HttpGet]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 60)]
        public FileContentResult DownloadFotoramaJs()
        {
            return File(System.IO.File.ReadAllBytes(Server.MapPath("~/Content/fotorama/fotorama.js")), "text/javascript");
        }
    }
}