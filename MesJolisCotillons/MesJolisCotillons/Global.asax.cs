using MesJolisCotillons.App_Start;
using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MesJoliesCotillons
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;
        }

        private void Application_EndRequest(object source, EventArgs e)
        {
            MesJoliesCotillonsDBContextManager.RemoveCurrentDbContext();

            if (Context.Response.StatusCode == 302 && Context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 403;
            }
        }
        
        protected void Session_Start(Object sender, EventArgs e)
        {
            var command = HttpContext.Current.Session["Command"];
            if (command == null)
            {
                //command = new Command();
                command = new MySessionCartModel();
                HttpContext.Current.Session["Command"] = command;
            }
        }


        protected void Session_End(Object sender, EventArgs e)
        {
            //var productRepository = new Product_Repository();
            //var command = HttpContext.Current?.Session["Command"];

            var command = this.Session["Command"];

            if (command != null)
            {
                var cartModel = (MySessionCartModel)command;
                cartModel.resetCart(doRechargeStockQuantity: true);

                //productRepository.Save();   // Save is done in clearProductsFromCart, because reinstanciante productRepository does not keep changes in clearProductsFromCart
            }
        }

        ////protected void Application_BeginRequest(object sender, EventArgs e)
        ////{
        ////    if (Request.IsMobileBrowser())
        ////    {
        ////        HttpContext.Current.RewritePath("/Information/CannotHandleMobile");
        ////        //HttpContext.Current.Response.Redirect("/Information/CannotHandleMobile");
        ////    }        
        ////}
    }
}
