using MesJolisCotillons.Area._Admin;
using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static ProductsPageRequest;
using MesJolisCotillons.Service;

namespace MesJolisCotillons.Area.Controllers
{
    public class ProductController : Controller
    {
        Product_Repository product_Repository = new Product_Repository();
        Util_Repository util_Repository = new Util_Repository();
        Design_Repository design_Repository = new Design_Repository();
        ProductItemViewService ProductItemViewService;

        public ProductController()
        {
            this.ProductItemViewService = new ProductItemViewService(product_Repository);
        }

        [AjaxOrChildActionOnly]
        [HttpGet]
        public ActionResult GetImageBlob(Int32 id)
        {
            var blob = util_Repository.GetBlob(id);
            ImageResult result = new ImageResult(blob.Stream, blob.MimeType);

            return result;
        }

        [HttpGet]
        public ActionResult GetFirstProductImage(string fileName)
        {
            var idString = fileName.Split('.');
            var idInt = Int32.Parse(idString[0].ToString());

            var product = product_Repository.FindProduct(idInt);

            if (product == null || product.Status < Product_Status.Enabled)
            {
                return null;
            }

            var firstBlob = product.Blob_Set.FirstOrDefault();
            if (firstBlob == null)
            {
                return null;
            }

            return GetImageBlob(firstBlob.Blob_ID);
        }

        #region Product

        #region PartialView

        [AjaxOrChildActionOnly]
        public ActionResult ProductsPartialView(ProductsViewModel productsViewModel)
        {
            if (productsViewModel.query.ProductsViewId == null)
            {
                var viewId = productsViewModel.getProductViewModelId();
                productsViewModel.query.ProductsViewId = viewId;
            }
            return PartialView("~/Views/Product/Products.cshtml", productsViewModel);
        }

        [AjaxOrChildActionOnly]
        public ActionResult ProductsGridPartialView(ProductsPageRequest query)
        {
            //query.pageNumber = Int32.Parse(pageNumber);
            var viewModel = new ProductsGridViewModel
            {
                query = query
            };

            var products = product_Repository.FindAllProduct().Where(item => item.Status == Product_Status.Enabled);

            List<Product> resultList = new List<Product>();
            bool hasBeenFiltered = false;

            if (query.hasFilter("Category_ID"))
            {
                var categoryFilters = query.filters.Where(item => item.property == "Category_ID")
                    .Select(item => item.value);
                var categoryIdListInt = categoryFilters.Select(item => int.Parse(item)).ToList();

                List<Product> list = products.Where(item =>
                    item.Category_Set.Select(c => c.Category_ID).Intersect(categoryIdListInt).Count() > 0).ToList();
                resultList.AddRange(list);

                hasBeenFiltered = true;
            }

            if (query.hasFilter("KeyWord"))
            {
                var keyWordList = query.filters.Where(item => item.property == "KeyWord").Select(item => item.value);

                List<Product> list = products
                    .Where(item => item.KeyWord_Set.Select(k => k.Value).Intersect(keyWordList).Count() > 0).ToList();
                resultList.AddRange(list);
                hasBeenFiltered = true;
            }


            if (query.hasFilter("Exclude_Product_ID"))
            {
                var excludeProductIdList = query.filters.Where(item => item.property == "Exclude_Product_ID")
                    .Select(item => item.value);
                resultList = resultList.Where(item => !excludeProductIdList.Contains(item.Product_ID.ToString()))
                    .ToList();
            }

            if (query.hasFilter("RandomOrder"))
            {
                resultList = resultList.OrderBy(a => Guid.NewGuid()).ToList();
            }

            if (query.hasFilter("OtherSameProducts"))
            {
                resultList = resultList.Take(query.pageSize).ToList();
                if (resultList.Count == 0)
                {
                    resultList = products.OrderBy(item => Guid.NewGuid())
                        .Take(query.pageSize)
                        .ToList();
                }
            }

            if (!hasBeenFiltered)
            {
                //var keyWordList = query.filters.Where(item => item.property == "KeyWord").Select(item => item.value);
                //products.Where(item => item.KeyWords.Select(k => k.Value).Intersect(keyWordList).Count() > 0).ToList();
                resultList = products.ToList();
            }

            resultList = resultList.Distinct().ToList();
            var totalOfItems = resultList.Count();

            #region paging

            if (!query.hasFilter("RandomOrder"))
            {
                resultList = resultList.OrderByDescending(item => item.Product_ID)
                    .Skip(query.start)
                    .Take(query.pageSize)
                    .ToList();
            }

            #endregion

            viewModel.products = resultList;
            viewModel.pagingInfos = new PagingInfo
            {
                totalItems = totalOfItems,
                pageSize = query.pageSize,
                pageNumber = query.pageNumber
            };

            return PartialView("~/Views/Product/ProductsGrid.cshtml", viewModel);
        }

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult OtherSameProductsGridPartialViewAjax(int Product_ID)
        {
            var viewModel = new ProductsViewModel
            {
                title = "VOUS AIMEREZ PEUT ETRE AUSSI...",
                width = "100%",
                //height="20%"
            };
            var product = product_Repository.FindProduct(Product_ID, Product_Status.Enabled);
            if (product == null)
            {
                return Json(new
                {
                    success = false
                });
            }

            var filters = new List<QueryFilter>();
            foreach (var category in product.Category_Set)
            {
                filters.Add(new QueryFilter
                {
                    property = "Category_ID",
                    value = category.Category_ID.ToString()
                });
            }

            foreach (var keyWord in product.KeyWord_Set)
            {
                filters.Add(new QueryFilter
                {
                    property = "KeyWord",
                    value = keyWord.Value
                });
            }

            filters.Add(new QueryFilter
            {
                property = "Exclude_Product_ID",
                value = Product_ID.ToString()
            });

            filters.Add(new QueryFilter
            {
                property = "RandomOrder",
                value = true.ToString()
            });

            filters.Add(new QueryFilter
            {
                property = "OtherSameProducts",
                value = true.ToString()
            });

            viewModel.query = new ProductsPageRequest
            {
                pageSize = 4,
                filters = filters,
                productGridCls = "otherInterestedProductGrid",
                scrollUpToHeader = false,
                displayPagingToolbar = false
            };
            return PartialView("~/Views/Product/OtherInterestingProducts.cshtml", viewModel);
        }


        [AjaxOrChildActionOnly]
        public ActionResult NouveautesProductsPartialView()
        {
            return PartialView("~/Views/Product/Products.cshtml", null);
        }

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult ProductsGridPartialViewAjax(ProductsPageRequest query)
        {
            return this.ProductsGridPartialView(query);
        }

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult ProductItemAddButtonAndQuantityInfoHtml(int Product_ID)
        {
            var product = product_Repository.FindProduct(Product_ID);

            if (product == null || product.Status < Product_Status.Enabled)
            {
                return Json(new
                {
                    success = false,
                    msg = "Le produit que vous recherchez ne semble plus exister"
                });
            }

            return Json(new
            {
                success = true,
                AddButtonAndQuantityInfoHtml = new
                {
                    AddCartButtonView = this.ProductItemViewService.GetAddCartButtonView(product.ProductStockFlag),
                    ProductQuantityInfoView =
                        this.ProductItemViewService.GetProductQuantityInfoView(product.ProductStockFlag)
                }
            });
        }

        #endregion

        #region View

        [HttpGet]
        public ActionResult GetProductView(string id)
        {
            int productId = 0;
            var isParseOk = Int32.TryParse(id, out productId);
            if (!isParseOk)
            {
                return Redirect("/");
            }

            string baseUrl = this.Request.Url.GetLeftPart(UriPartial.Authority);
            string absoluteUri = this.Request.Url.AbsoluteUri;

            var viewModel = this.ProductItemViewService.GetProductViewResponse(productId, baseUrl, absoluteUri);
            if (viewModel == null)
            {
                return Redirect("/");
            }

            return View("~/Views/Product/ProductItem.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult ProductsByCategoryView(string id)
        {
            int categoryId = 0;
            var isParseOk = Int32.TryParse(id, out categoryId);
            if (!isParseOk)
            {
                return Redirect("/");
            }

            var category = product_Repository.FindCategory(categoryId);
            if (category == null)
            {
                return Redirect("/");
            }

            var categoryViewModel = new CategoryViewModel
            {
                title = category.DisplayName,
                Category_ID = category.Category_ID,
                etiquette = category?.Etiquette
            };

            return View("~/Views/Product/ProductsByCategory.cshtml", categoryViewModel);
        }

        public ActionResult ProductsByCategoryNameView(string id)
        {
            var category = product_Repository.FindAllCategory().Where(item => item.DisplayName == id).FirstOrDefault();
            if (category == null)
            {
                return Redirect("/");
            }

            var productsViewModel = new ProductsViewModel
            {
                title = id
            };

            if (category != null)
            {
                productsViewModel.query = new ProductsPageRequest
                {
                    filters = new List<QueryFilter>()
                        {new QueryFilter {property = "Category_ID", value = category.Category_ID.ToString()}}
                };
            }

            return View("~/Views/Product/Products.cshtml", productsViewModel);
        }

        public ActionResult ProductsByMenuItemIdView(string id)
        {
            var designConfigXmlData = design_Repository.getActiveDesignConfigXmlFromSession(Session);
            var headerMenuItem = designConfigXmlData.FindHeaderMenuItem(id);

            if (headerMenuItem == null || !headerMenuItem.EnableClickFilter)
            {
                return Redirect("/");
            }

            if (headerMenuItem is UrlMenuItem)
            {
                string url = ((UrlMenuItem) headerMenuItem).Url;
                if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url))
                {
                    return (ActionResult) this.Redirect("/");
                }

                return (ActionResult) this.Redirect(url);
            }

            IProductViewModel productViewModel = headerMenuItem as IProductViewModel;
            if (productViewModel == null)
            {
                return (ActionResult) this.Redirect("/");
            }


            var model = productViewModel.getProductsViewModel();
            if (model.query.ProductsViewId == null)
            {
                var viewId = model.getProductViewModelId();
                model.query.ProductsViewId = viewId;
            }

            return (ActionResult) this.View("~/Views/Product/Products.cshtml", model);
        }

        #endregion

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult updateProductQuantity(ProductAvailability form)
        {
            var command = (MySessionCartModel) Session["Command"];
            if (command == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Il semble y avoir un problème avoir votre session"
                });
            }

            var product = product_Repository.FindProduct(form.Product_ID, Product_Status.Enabled);
            if (product == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Le produit que vous recherchez n'est plus disponible"
                });
            }

            var sessionProduct = command.commandProducts.Where(item => item.product.Product_ID == form.Product_ID)
                .FirstOrDefault();

            if (sessionProduct == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Il semble y avoir un problème avoir votre session"
                });
            }

            if ((product.StockQuantity + sessionProduct.quantity) - form.Quantity < 0)
            {
                return Json(new
                {
                    success = false,
                    msg = "Le produit que vous recherchez n'est plus disponible pour la quantité souhaitée"
                });
            }

            product.StockQuantity += sessionProduct.quantity;
            sessionProduct.quantity = 0;
            //command.commandProducts.Add(sessionProduct);
            Session["Command"] = command;

            return addProductOnCart(form.Product_ID, form.Quantity);
        }

        #endregion

        #region Cart

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult addProduct(Int32 Product_ID)
        {
            return addProductOnCart(Product_ID);
        }

        #endregion

        private ActionResult addProductOnCart(int Product_ID, int Quantity = 1)
        {
            var product = product_Repository.FindAllProduct()
                .Where(item => item.Product_ID == Product_ID)
                .FirstOrDefault();

            #region Ontology

            if (product == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Le produit que vous recherchez ne semble plus exister"
                });
            }

            if (product.Status != Product_Status.Enabled)
            {
                return Json(new
                {
                    success = false,
                    msg = "Le produit que vous recherchez n'est plus disponible"
                });
            }

            if (product.StockQuantity == 0)
            {
                return Json(new
                {
                    success = false,
                    msg = "Le produit que vous recherchez n'est plus disponible"
                });
            }

            if (product.StockQuantity - Quantity < 0)
            {
                return Json(new
                {
                    success = false,
                    msg = "Le produit que vous recherchez n'est plus disponible pour la quantité souhaitée"
                });
            }

            #endregion

            var command = (MySessionCartModel) Session["Command"];
            if (command == null)
            {
                command = new MySessionCartModel();
            }

            #region Weight Cart Ontology

            var totalCarWeightWithProduct = command.TotalCartPackagedWeight +
                                            (Quantity * product.Product_XmlData.ProductPackagedWeight);
            if (totalCarWeightWithProduct > 30000)
            {
                return Json(new
                {
                    success = false,
                    msg = "La limite du panier est de 30kgs, vous ne pouvez pas ajouter le/les produit(s) souhaité(s)"
                });
            }

            #endregion

            var sessionProduct = command.commandProducts.Where(item => item.product.Product_ID == Product_ID)
                .FirstOrDefault();
            if (sessionProduct == null)
            {
                var commandProduct = new CommandProductModel();
                var producModel = new ProductItemModel
                {
                    Product_ID = Product_ID,
                    Name = product.Name,
                    DisplayName = product.DisplayName,
                    Price = (decimal) product?.Price,
                    BlobsIds = product.Blob_Set.Select(item => item.Blob_ID).ToList(),
                    ProductPackagedWeight = product.Product_XmlData.ProductPackagedWeight,
                    ProductType = product.Product_XmlData.ProductType
                };

                commandProduct.quantity = Quantity;
                commandProduct.product = producModel;
                command.commandProducts.Add(commandProduct);
            }
            else
            {
                sessionProduct.quantity += Quantity;
            }
            //decimal deliveryCharge = new LaPosteAPI().computeDeliveryChargeForCart(command);
            //command.DeliveryCharge = deliveryCharge;

            product.StockQuantity -= Quantity;
            Session["Command"] = command;
            product_Repository.Save();


            return Json(new
            {
                success = true
            });
        }
    }
}