using MesJolisCotillons.Area.Cart.Service;
using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MesJolisCotillons.Area.Cart.Controllers
{
    public class CartController : Controller
    {
        private readonly Product_Repository product_Repository = new Product_Repository();
        private readonly CartViewService CartViewService;

        public CartController()
        {
            this.CartViewService = new CartViewService(this.product_Repository);
        }

        #region View
        public ActionResult MyCartView()
        {
            return View("~/Views/Cart/MyCart.cshtml");
        }
        #endregion

        #region PartialView
        [AjaxOrChildActionOnly]
        public ActionResult MyCartGridPartialView()
        {
            var command = Session["Command"];
            MySessionCartModel myCart = null;

            if (command == null)
            {
                Redirect("/");
            }

            myCart = (MySessionCartModel)command;
            var carViewModel = this.CartViewService.GetCartViewModel(myCart);
            return PartialView("~/Views/Cart/MyCartGrid.cshtml", carViewModel);
        }

        #endregion

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult RemoveFromMyCart(Int32 Product_ID)
        {
            var command = (MySessionCartModel)Session["Command"];
            var commandProduct = command.commandProducts
                        .Where(item => item.product.Product_ID == Product_ID)
                        .FirstOrDefault();
            var quantity = commandProduct.quantity;

            command.commandProducts.Remove(commandProduct);
            Session["Command"] = command;

            var product = product_Repository.FindAllProduct()
                .Where(item => item.Product_ID == Product_ID)
                .FirstOrDefault();

            if (product == null)
            {
                throw new Exception("RemoveFromMyCart: le produit ne semble plus exister");
            }

            if (product.Product_XmlData.ProductType == ProductType.SERIE)
            {
                product.StockQuantity += quantity;
            }
            else if (product.Product_XmlData.ProductType == ProductType.UNIQUE)
            {
                product.StockQuantity = 1;
            }


            product_Repository.Save();

            return Json(new
            {
                success = true
            });
        }

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult getActualCartTotal()
        {
            var command = Session["Command"];
            var totalProductCount = 0;
            string totalCartPrice = "0.00";

            if (command != null)
            {
                MySessionCartModel cmd = ((MySessionCartModel)command);
                totalProductCount = cmd.TotalProductCount;
                totalCartPrice = cmd.TotalCommand != null ? ((decimal)cmd.TotalCommand).ToString("0.00") : "0.00";
            }
            return Json(new
            {
                success = true,
                totalCount = totalProductCount,
                totalCartPrice = totalCartPrice
            });
        }
    }
}