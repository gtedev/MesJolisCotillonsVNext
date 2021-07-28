using MesJolisCotillons.Extensions.Paypal;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesJolisCotillons.Area.PaypalNotification.Controllers
{
    public class PaypalController : Controller 
    {
        private Util_Repository util_Repository = new Util_Repository();

        // GET: Paypal_
        [HttpGet]
        public ActionResult GetMesJolisCotillonsSimpleLogo()
        {
            var imageLogoPath = PaypalConfig.PaypalConfigDependingOnMode["imageLogoPath"]; 
            string path = HttpContext.Server.MapPath(imageLogoPath);

            byte[] simpleLogoImg = util_Repository.GetImageFromPath(path);
            ImageResult result = new ImageResult(simpleLogoImg, "png");

            return result;
        }
    }
}