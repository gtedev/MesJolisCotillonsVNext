using MesJolisCotillons.Extensions.LaPoste;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesJolisCotillons.Area.LaPosteColissimo.Controllers
{
    public class LaPosteColissimoController : Controller
    {
        LaPosteColissimo_Repository laPoste_Repository = new LaPosteColissimo_Repository();

        //public ActionResult createUpdateLaPosteColissimoTable()
        //{

        //    for (int i = 1; i <= 250; i++)
        //    {
        //        decimal value = (decimal)4.9;
        //        laPoste_Repository.createAndAddLaPosteCost(i, value);
        //    }

        //    for (int i = 251; i <= 500; i++)
        //    {
        //        decimal value = (decimal)6.1;
        //        laPoste_Repository.createAndAddLaPosteCost(i, value);
        //    }

        //    for (int i = 501; i <= 750; i++)
        //    {
        //        decimal value = (decimal)6.9;
        //        laPoste_Repository.createAndAddLaPosteCost(i, value);
        //    }

        //    for (int i = 751; i <= 1000; i++)
        //    {
        //        decimal value = (decimal)7.5;
        //        laPoste_Repository.createAndAddLaPosteCost(i, value);
        //    }

        //    for (int i = 1001; i <= 2000; i++)
        //    {
        //        decimal value = (decimal)8.5;
        //        laPoste_Repository.createAndAddLaPosteCost(i, value);
        //    }

        //    for (int i = 2001; i <= 5000; i++)
        //    {
        //        decimal value = (decimal)12.9;
        //        laPoste_Repository.createAndAddLaPosteCost(i, value);
        //    }

        //    for (int i = 5001; i <= 10000; i++)
        //    {
        //        decimal value = (decimal)18.9;
        //        laPoste_Repository.createAndAddLaPosteCost(i, value);
        //    }

        //    for (int i = 10001; i <= 30000; i++)
        //    {
        //        decimal value = (decimal)26.9;
        //        laPoste_Repository.createAndAddLaPosteCost(i, value);
        //    }

        //    laPoste_Repository.Save();

        //    return Json(new
        //    {
        //        success = true,
        //        message = "createUpdateLaPosteColissimoTable: SUCCESS"
        //    }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult computeWithLaPosteAPI(int id)
        {


            decimal priceDecimal = 0;
            var result = new LaPosteAPI().TarifEnvoi(id, TypeEnvoi.colis);
            if (!result.isSuccess())
            {
                throw new Exception("Error during LaPosteAPI for getting TarifEnvoi");
            }

            var itemBureau = result.Tarifs_Set.Where(item => item.channel.Equals("bureau")).FirstOrDefault();
            if (itemBureau == null)
            {
                throw new Exception("TarifEnvoi does not contain expected response");
            }

            var isParseOk = Decimal.TryParse(itemBureau.price, NumberStyles.Any, new CultureInfo("en-US"), out priceDecimal);


            return Json(new
            {
                success = true,
                message = "LaPoste prix: " + id + ":" + priceDecimal + " €"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}