using Ext.Direct.Mvc;
using MesJolisCotillons.Extensions;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesJolisCotillons.Area._Admin.Controllers.Direct
{
    [AdminAuthorize]
    [DirectHandleError]
    public class ProductDirectController : DirectController
    {
        Product_Repository product_Repository = new Product_Repository();
        Util_Repository util_Repository = new Util_Repository();

        public ActionResult getProduct_Set(DirectStoreQuery q)
        {
            var products_Set = product_Repository.FindAllProduct();

            #region filter
            if (q.filterHasProperty("SearchProductFilter"))
            {
                var value = q.filterValue("SearchProductFilter").Trim();

                int productId;
                bool productIdOk = Int32.TryParse(value, out productId);

                if (productIdOk)
                {
                    products_Set = products_Set.Where(item => item.Product_ID == productId);
                }
                else
                {
                    products_Set = products_Set.Where(item => item.Category_Set.Any(c => c.DisplayName == value) || item.DisplayName.Contains(value) || item.Name.Contains(value) || item.Description.Contains(value));
                }

            }
            #endregion

            #region orderby
            //products_Set = products_Set.OrderBy(item => item.Product_ID);
            products_Set = products_Set.OrderBy(item => item.Name);

            #endregion

            #region paging
            int totalProduct = products_Set.Count();
            if (q.start != null && q.limit != null)
            {
                products_Set = products_Set.Skip(q.start.Value).Take(q.limit.Value);
            }
            #endregion

            var results = products_Set.AsEnumerable().Select(item => new
            {
                item.Product_ID,
                item.Name,
                item.DisplayName,
                item.Description,
                item.Price,
                Category_FK = item.Category_Set.Count() > 0 ? (int?)item.Category_Set.FirstOrDefault()?.Category_ID : null,
                Category = item.Category_Set.Count() > 0 ? item.Category_Set.Select(c => c.Name).Aggregate((a, b) => a + ", " + b) : null,
                Status = item.Status.ToString(),
                item.CurrentUserAbility.userCanActiveProduct,
                item.CurrentUserAbility.userCanDeactivateProduct,
            }).ToList();

            return Json(new
            {
                total = totalProduct,
                data = results
            });
        }

        public ActionResult getCategory_Set()
        {
            var categories = product_Repository.FindAllCategory();
            categories = categories.OrderBy(item => item.Name);

            var result = categories.Select(item => new
            {
                item.Category_ID,
                Category_FK = item.Category_ID,
                item.Name,
                item.DisplayName,
                item.Etiquette_FK,
                EtiquetteName = item.Etiquette == null ? "" : item.Etiquette.Etiquette_Name
            }).ToList();

            return Json(new
            {
                success = true,
                data = result
            });
        }

        public ActionResult getProduct(int id)
        {
            Product product = this.product_Repository.FindProduct(id);
            if (product == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Produit non trouvée"
                });
            }

            var result = new
            {
                product.Product_ID,
                product.Name,
                product.DisplayName,
                Category = product.Category_Set.Count() > 0 ? product.Category_Set.Select(c => c.Name).Aggregate((a, b) => a + ", " + b) : null,
                product.Description,
                product.Price,
                product.StockQuantity,
                Category_FK = product.Category_Set.Count() > 0 ? (int?)product.Category_Set.FirstOrDefault().Category_ID : null,
                ProductKeyWords_Set = product.KeyWord_Set.Select(item => item.KeyWord_ID).ToList(),
                ProductCategory_Set = product.Category_Set.Select(item => item.Category_ID).ToList(),
                ProductWeight = product.Product_XmlData.Weight,
                ProductHeight = product.Product_XmlData.Height,
                ProductWidth = product.Product_XmlData.Width,
                ProductDepth = product.Product_XmlData.Depth,
                ProductDiameter = product.Product_XmlData.Diameter,
                Type = product.Product_XmlData.ProductType,
                product.Product_XmlData.ProductFragility,
                product.Product_XmlData.ProductDeliveryBoxType
            };

            return Json(new
            {
                success = true,
                data = result
            });
        }

        public ActionResult getCategory(int id)
        {
            var category = this.product_Repository.FindCategory(id);

            if (category == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Catégorie non trouvée"
                });
            }


            var result = new
            {
                category.Category_ID,
                Category_FK = category.Category_ID,
                category.Name,
                category.DisplayName,
                category.Etiquette_FK
            };

            return Json(new
            {
                success = true,
                data = result
            });
        }

        public ActionResult getProductImages_Set(DirectStoreQuery q)
        {
            Product product = null;
            if (q.filterHasProperty("Product_ID"))
            {
                int productId = Int32.Parse(q.filterValue("Product_ID"));
                product = product_Repository.FindProduct(productId);
            }

            var result = product.Blob_Set.Select(item => new
            {
                product.Product_ID,
                item.Blob_ID,
                item.FileName,
                FileSizeKbytes = item.Stream.Length / 1024,
                IsMainBbob = item.Blob_ID == product.Main_Blob_FK,
                item.CreationDateTime
            }).ToList();

            return Json(new
            {
                success = true,
                data = result
            });
        }

        [FormHandler]
        public ActionResult uploadProductImage(int Product_ID, int compressionQuality = 100)
        {
            HttpPostedFileBase hpf;

            #region Ontology
            var files = Request.Files;
            if (Request.Files.Count < 1)
                throw new Exception("Aucun fichier n'a été soumis !");
            if (Request.Files.Count > 1)
                throw new Exception("Plus de 1 ficheir a été soumis !");

            hpf = Request.Files[0];
            if (hpf.ContentLength == 0)
                throw new Exception("Le fichier est-il vide ? ?");

            if (hpf.ContentLength > 10000000)
            {
                return Json(new
                {
                    msg = "Le fichier est trop large, il doit être en dessous de 10MB",
                    success = false
                });
            }

            var product = product_Repository.FindProduct(Product_ID);
            if (product == null)
            {
                return Json(new
                {
                    msg = Product_ID + " ne semble plus exister",
                    success = false
                });
            }
            #endregion

            var blob = new Blob();
            blob.CreationDateTime = DateTime.Now;
            blob.FileName = hpf.FileName;
            blob.MimeType = hpf.ContentType;

            byte[] binaryData = new byte[hpf.ContentLength];
            hpf.InputStream.Read(binaryData, 0, hpf.ContentLength);
            blob.Stream = binaryData;

            //Bitmap newBitmap;
            //using (MemoryStream memoryStream = new MemoryStream(binaryData))
            //using (Image newImage = Image.FromStream(memoryStream))
            //{
            //    newBitmap = new Bitmap(newImage);
            //}

            //compressionQuality = 99;
            //if (compressionQuality < 100) 
            //{
            //    //var steam = ImageUtils.CompressImage2(newBitmap, 80, ImageCodecInfo.GetImageEncoders()[0]);
            //    var steam = ImageUtils.CompressJpegImage(newBitmap, 30);
            //    var binaryData2 = new byte[steam.Length];
            //    steam.Write(binaryData2, 0, (int)steam.Length);
            //    blob.Stream = binaryData2;
            //}
            product.Blob_Set.Add(blob);


            product_Repository.Save();

            return Json(new
            {
                msg = "Image uploadé avec succès",
                success = true
            });


        }

        public ActionResult deleteProductImage(int Product_ID, int Blob_ID)
        {
            var product = product_Repository.FindProduct(Product_ID);
            if (product == null)
            {
                return Json(new
                {
                    msg = Product_ID + " ne semble plus exister",
                    success = false
                });
            }

            var blob = product.Blob_Set.Where(item => item.Blob_ID == Blob_ID).FirstOrDefault();
            if (blob == null)
            {
                return Json(new
                {
                    msg = Blob_ID + " ne semble plus exister",
                    success = false
                });
            }

            product.Blob_Set.Remove(blob);
            product_Repository.deleteBlob(blob);
            product_Repository.Save();

            return Json(new
            {
                msg = "Image supprimée avec succès.",
                success = true
            });
        }

        [FormHandler]
        public ActionResult createProduct(Product productForm, List<int> ProductKeyWords_Set, List<int> ProductCategory_Set, int ProductWeight, int? ProductHeight, int? ProductWidth, int? ProductDepth, int? ProductDiameter, ProductType Type, ProductFragility ProductFragility, ProductDeliveryBoxType ProductDeliveryBoxType)
        {
            Product product = null;
            if (String.IsNullOrEmpty(productForm.Name))
            {
                return Json(new
                {
                    msg = "Le nom du produit dans le formulaire soumis est obligatoire",
                    success = false
                });
            }

            product = new Product();
            product.Name = productForm.Name.Trim();

            if (!String.IsNullOrEmpty(productForm.Description))
            {
                product.Description = productForm.Description.Trim();
            }

            if (productForm.Price != null)
            {
                product.Price = productForm.Price;
            }

            if (productForm.StockQuantity != null)
            {
                product.StockQuantity = productForm.StockQuantity;
            }
            else
            {
                product.StockQuantity = 1;
            }

            #region ProductXmlData

            var productXmlData = new Product_XmlData();
            if (ProductWeight > 0)
            {
                productXmlData.Weight = ProductWeight;
            }
            if (ProductHeight > 0)
            {
                productXmlData.Height = (int)ProductHeight;
            }
            if (ProductWidth > 0)
            {
                productXmlData.Width = (int)ProductWidth;
            }
            if (ProductDepth > 0)
            {
                productXmlData.Depth = (int)ProductDepth;
            }

            if (ProductDiameter > 0)
            {
                productXmlData.Diameter = (int)ProductDiameter;
            }

            productXmlData.ProductType = Type;
            if (productXmlData.ProductType == ProductType.UNIQUE)
            {
                product.StockQuantity = 1;
            }
            productXmlData.ProductFragility = ProductFragility;
            productXmlData.ProductDeliveryBoxType = ProductDeliveryBoxType;

            product.xmlData = productXmlData.Serialize();
            #endregion

            if (!String.IsNullOrEmpty(productForm.DisplayName))
            {
                product.DisplayName = productForm.DisplayName.Trim();
            }

            if (productForm.Category_Set != null)
            {
                foreach (int categoryId in ProductCategory_Set)
                {
                    var category = product_Repository.FindCategory(categoryId);
                    if (category != null && !product.Category_Set.Any(c => c.Category_ID == category.Category_ID))
                    {
                        product.Category_Set.Add(category);
                    }
                }
            }
            if (ProductKeyWords_Set != null)
            {
                foreach (int keyWordId in ProductKeyWords_Set)
                {
                    var keyWord = product_Repository.FindKeyWord(keyWordId);
                    if (keyWord != null && !product.KeyWord_Set.Any(k => k.KeyWord_ID == keyWord.KeyWord_ID))
                    {
                        product.KeyWord_Set.Add(keyWord);
                    }
                }
            }


            product_Repository.addProduct(product);
            product_Repository.Save();

            return Json(new
            {
                msg = "Produit créé avec succès.",
                success = true
            });
        }

        public ActionResult activateProduct(int Product_ID)
        {
            var product = product_Repository.FindProduct(Product_ID);

            #region Ontology
            if (product == null)
            {
                return Json(new
                {
                    msg = Product_ID + " ne semble plus exister",
                    success = false
                });
            }
            if (!product.CurrentUserAbility.userCanActiveProduct)
            {
                return Json(new
                {
                    msg = "Le produit ne peut pas être activé",
                    success = false
                });
            }
            #endregion

            product.Status = Product_Status.Enabled;

            product_Repository.Save();
            return Json(new
            {
                msg = "Produit activé avec succès.",
                success = true
            });
        }

        public ActionResult deactivateProduct(int Product_ID)
        {

            var product = product_Repository.FindProduct(Product_ID);

            #region Ontology
            if (product == null)
            {
                return Json(new
                {
                    msg = Product_ID + " ne semble plus exister",
                    success = false
                });
            }
            if (!product.CurrentUserAbility.userCanDeactivateProduct)
            {
                return Json(new
                {
                    msg = "Le produit ne peut pas être désactivé",
                    success = false
                });
            }
            #endregion

            product.Status = Product_Status.Disabled;

            product_Repository.Save();
            return Json(new
            {
                msg = "Produit désactivé avec succès.",
                success = true
            });
        }

        [FormHandler]
        public ActionResult createCategory(Category categoryForm)
        {
            #region Ontology
            var isCategoryExist = product_Repository.FindAllCategory().Any(item => item.Name == categoryForm.Name);
            if (isCategoryExist)
            {
                return Json(new
                {
                    msg = "La catégorie" + categoryForm.Name + "existe déjà !",
                    success = false
                });
            }
            if (String.IsNullOrEmpty(categoryForm.Name))
            {
                return Json(new
                {
                    msg = "La nom catégorie est manquant !",
                    success = false
                });
            }
            #endregion

            var category = new Category();
            category.Name = categoryForm.Name;

            if (!String.IsNullOrEmpty(categoryForm.DisplayName))
            {
                category.DisplayName = categoryForm.DisplayName;
            }

            if (categoryForm.Etiquette_FK != null)
            {
                category.Etiquette_FK = categoryForm.Etiquette_FK;
            }

            product_Repository.addCategory(category);

            product_Repository.Save();
            return Json(new
            {
                msg = "Catégorie créé avec succès.",
                success = true
            });
        }

        public ActionResult deleteProduct(int Product_ID)
        {
            var product = product_Repository.FindProduct(Product_ID);

            #region Ontology
            if (product == null)
            {
                return Json(new
                {
                    msg = Product_ID + " ne semble plus exister",
                    success = false
                });
            }
            if (!product.CurrentUserAbility.userCanDeleteProduct)
            {
                return Json(new
                {
                    msg = "<div style=\"text-align:left;\">Le produit ne peut pas être supprimé. </br></br> Raisons éventuels:<ul><li>Statut produit  => ne doit pas être Enabled</li><li>Produit déjà lié à des commandes...</li></ul></div>",
                    success = false
                });
            }
            #endregion

            product_Repository.deleteProduct(product);
            product_Repository.Save();

            return Json(new
            {
                msg = "Produit supprimé avec succès.",
                success = true
            });
        }

        public ActionResult getProductFlags_Set()
        {
            var results = Enum.GetValues(typeof(Product_Flags)).Cast<Product_Flags>().Select(item => new
            {
                Flag = item,
                DisplayFlag = item.ToString(),
            });

            return Json(new
            {
                success = true,
                data = results
            });
        }


        [FormHandler]
        public ActionResult deleteCategory(Category categoryForm)
        {
            Category category = product_Repository.FindAllCategory()
                                                  .Where(item => item.Category_ID == categoryForm.Category_ID)
                                                  .FirstOrDefault();
            #region Ontology

            if (category == null)
            {
                return Json(new
                {
                    msg = ("La catégorie" + categoryForm.Name + "n'existe pas !"),
                    success = false
                });
            }

            if (category.isUseSomewhere)
            {
                return (ActionResult)this.Json((object)new
                {
                    msg = ("<div style=\"text-align:left;\">Cette catégorie " + category.Name + " (Category_ID: " + (object)category.Category_ID + ") ne peut pas être supprimé, car elle est utilisée par d'autres données. </br></br> Endroits éventuels:<ul><li>Design Configuration</li><li>Produit</li></ul></div>"),
                    success = false
                });
            }
            #endregion

            product_Repository.deleteCategory(category);
            product_Repository.Save();

            return Json(new
            {
                msg = "Catégorie a été supprimée avec succès.",
                success = true
            });
        }

        [HttpGet]
        public ActionResult GetImageBlob(int id)
        {
            Blob blob = util_Repository.GetBlob(id);

            return new ImageResult(blob.Stream, blob.MimeType);
        }

        public ActionResult getKeyWord_Set()
        {
            IQueryable<KeyWord> allKeyWord = this.product_Repository.FindAllKeyWord();

            var results = allKeyWord.Select(item => new
            {
                KeyWord_ID = item.KeyWord_ID,
                Value = item.Value
            }).ToList();

            return Json(new
            {
                success = true,
                data = results
            });
        }

        [FormHandler]
        public ActionResult deleteKeyWord(KeyWord keyWordForm)
        {
            KeyWord keyWord = product_Repository.FindAllKeyWord()
                                                .Where(item => item.KeyWord_ID == keyWordForm.KeyWord_ID)
                                                .FirstOrDefault();
            if (keyWord == null)
            {
                return Json(new
                {
                    msg = ("Le mot clé" + keyWordForm.Value + "n'existe pas !"),
                    success = false
                });
            }

            if (keyWord.isUseSomewhere)
            {
                return Json(new
                {
                    msg = ("<div style=\"text-align:left;\">Ce mot clé " + keyWord.Value + " (KeyWord_ID: " + keyWord.KeyWord_ID + ") ne peut pas être supprimé, car il est utilisée par d'autres données. </br></br> Endroits éventuels:<ul><li>Design Configuration</li><li>Produit</li></ul></div>"),
                    success = false
                });
            }

            product_Repository.deleteKeyWord(keyWord);
            product_Repository.Save();

            return Json(new
            {
                msg = "Le mot clé a été supprimé avec succès.",
                success = true
            });
        }

        [FormHandler]
        public ActionResult createKeyWord(KeyWord keyWordForm)
        {
            if (product_Repository.FindAllKeyWord().Any(item => item.Value == keyWordForm.Value))
            {
                return Json(new
                {
                    msg = ("Le mot clé" + keyWordForm.Value + "existe déjà !"),
                    success = false
                });
            }

            if (String.IsNullOrEmpty(keyWordForm.Value))
            {
                return Json(new
                {
                    msg = "La valeur du mot clé est manquante !",
                    success = false
                });
            }

            product_Repository.addKeyWord(new KeyWord()
            {
                Value = keyWordForm.Value
            });
            product_Repository.Save();

            return Json(new
            {
                msg = "Mot clé créé avec succès.",
                success = true
            });
        }

        public ActionResult getProductKeyWords_Set()
        {
            IQueryable<KeyWord> allKeyWord = product_Repository.FindAllKeyWord();
            var results = allKeyWord.OrderBy(item => item.Value).Select(item => new
            {
                KeyWord_ID = item.KeyWord_ID,
                KeyWord = item.Value
            }).ToList();

            return Json(new
            {
                success = true,
                data = results
            });
        }

        [FormHandler]
        public ActionResult editInformationProduct(Product productForm, List<int> ProductKeyWords_Set, List<int> ProductCategory_Set, int ProductWeight, int? ProductHeight, int? ProductWidth, int? ProductDepth, int? ProductDiameter, ProductType Type, ProductFragility ProductFragility, ProductDeliveryBoxType ProductDeliveryBoxType)
        {
            var product = product_Repository.FindProduct(productForm.Product_ID);

            if (!String.IsNullOrEmpty(productForm.Name))
            {
                product.Name = productForm?.Name.Trim();
            }

            if (!String.IsNullOrEmpty(productForm.DisplayName))
            {
                product.DisplayName = productForm?.DisplayName.Trim();
            }

            if (!String.IsNullOrEmpty(productForm.Description))
            {
                product.Description = productForm?.Description.Trim();
            }

            if (productForm.Price != null)
            {
                product.Price = productForm?.Price;
            }

            if (productForm.StockQuantity != null)
            {
                product.StockQuantity = productForm.StockQuantity;
            }
            #region ProductXmlData

            var productXmlData = new Product_XmlData();
            if (ProductWeight > 0)
            {
                productXmlData.Weight = ProductWeight;
            }
            if (ProductHeight > 0)
            {
                productXmlData.Height = (int)ProductHeight;
            }
            if (ProductWidth > 0)
            {
                productXmlData.Width = (int)ProductWidth;
            }

            if (ProductDepth > 0)
            {
                productXmlData.Depth = (int)ProductDepth;
            }

            if (ProductDiameter > 0)
            {
                productXmlData.Diameter = (int)ProductDiameter;
            }

            productXmlData.ProductType = Type;
            if (productXmlData.ProductType == ProductType.UNIQUE)
            {
                if (product.StockQuantity >= 1)
                {
                    product.StockQuantity = 1;
                }
            }
            productXmlData.ProductFragility = ProductFragility;
            productXmlData.ProductDeliveryBoxType = ProductDeliveryBoxType;

            product.xmlData = productXmlData.Serialize();

            #endregion

            if (productForm.Category_Set != null)
            {
                product.Category_Set.Clear();
                foreach (int categoryId in ProductCategory_Set)
                {
                    var category = product_Repository.FindCategory(categoryId);
                    if (category != null && !product.Category_Set.Any(c => c.Category_ID == category.Category_ID))
                    {
                        product.Category_Set.Add(category);
                    }
                }
            }
            if (ProductKeyWords_Set != null)
            {
                product.KeyWord_Set.Clear();
                //foreach (KeyWord keyWord in product.KeyWords.ToList())
                //{
                //    product.KeyWords.Remove(keyWord);
                //}

                foreach (int keyWordId in ProductKeyWords_Set)
                {
                    var keyWord = product_Repository.FindKeyWord(keyWordId);
                    if (keyWord != null && !product.KeyWord_Set.Any(k => k.KeyWord_ID == keyWord.KeyWord_ID))
                    {
                        product.KeyWord_Set.Add(keyWord);
                    }
                }
            }
            product_Repository.Save();
            return Json(new
            {
                success = true,
                msg = "Produit édité avec succès"
            });
        }

        public ActionResult getProductType_Set()
        {
            var results = Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Select(item => new
            {
                ProductType = item,
                DisplayProductType = item.ToString(),
            });

            return Json(new
            {
                success = true,
                data = results
            });
        }

        public ActionResult getProductFragility_Set()
        {
            var results = Enum.GetValues(typeof(ProductFragility)).Cast<ProductFragility>().Select(item => new
            {
                ProductFragility = item,
                DisplayProductFragility = item.ToString(),
            });

            return Json(new
            {
                success = true,
                data = results
            });
        }

        public ActionResult getProductDeliveryBoxType_Set()
        {
            var results = Enum.GetValues(typeof(ProductDeliveryBoxType)).Cast<ProductDeliveryBoxType>().Select(item => new
            {
                ProductDeliveryBoxType = item,
                DisplayProductDeliveryBoxType = item.ToString(),
            });

            return Json(new
            {
                success = true,
                data = results
            });
        }

        [FormHandler]
        public ActionResult editCategory(Category categoryForm)
        {
            #region Ontology
            var category = product_Repository.FindCategory(categoryForm.Category_ID);

            if (category == null)
            {
                return Json(new
                {
                    msg = "La catégorie" + categoryForm.Name + "ne semble plus exister !",
                    success = false
                });
            }
            if (String.IsNullOrEmpty(categoryForm.Name))
            {
                return Json(new
                {
                    msg = "La nom catégorie est manquant !",
                    success = false
                });
            }
            #endregion

            category.Name = categoryForm.Name;

            if (!String.IsNullOrEmpty(categoryForm.DisplayName))
            {
                category.DisplayName = categoryForm.DisplayName;
            }

            if (categoryForm.Etiquette_FK != null)
            {
                category.Etiquette_FK = categoryForm.Etiquette_FK;
            }

            product_Repository.Save();
            return Json(new
            {
                msg = "Catégorie éditée avec succès.",
                success = true
            });
        }

    }
}