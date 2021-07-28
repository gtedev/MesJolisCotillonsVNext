using Ext.Direct.Mvc;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MesJolisCotillons.Area._Admin
{
    [AdminAuthorize]
    [DirectHandleError]
    public class DesignDirectController : DirectController
    {
        Design_Repository design_Repository = new Design_Repository();

        // GET: Admin_
        [AdminAuthorize]
        public ActionResult getDesign_Config_Set(DirectStoreQuery q)
        {
            var designConfigs_Set = design_Repository.FindAllDesign_Config();

            #region orderby
            designConfigs_Set = designConfigs_Set.OrderBy(item => item.Design_Config_ID);
            #endregion

            #region paging
            int totalProduct = designConfigs_Set.Count();
            if (q.start != null && q.limit != null)
            {
                designConfigs_Set = designConfigs_Set.Skip(q.start.Value).Take(q.limit.Value);
            }
            #endregion

            var results = designConfigs_Set.AsEnumerable().Select(item => new
            {
                item.Design_Config_ID,
                item.Name,
                item.Description,
                Status = item.Status.ToString(),
                CreationDatetime = item.CreationDatetime?.ToString("dd-MM-yyyy"),
                LastUpdateDatetime = item.LastUpdateDatetime?.ToString("dd-MM-yyyy"),
                canEditDesignConfig = item.CurrentUserAbility.canEditDesignConfig,
                canActiveDesignConfig = item.CurrentUserAbility.canActiveDesignConfig,
                canDeleteDesignConfig = item.CurrentUserAbility.canDeleteDesignConfig,
            }).ToList();

            return Json(new
            {
                total = totalProduct,
                data = results
            });
        }

        public ActionResult getDesign_ConfigTree(DirectStoreQuery q)
        {

            Design_Config designConfig = null;

            if (q.filterHasProperty("Design_Config_ID"))
            {

                int id = Int32.Parse(q.filterValue("Design_Config_ID"));
                designConfig = design_Repository.FindAllDesign_Config().Where(item => item.Design_Config_ID == id).FirstOrDefault();
            }

            if (designConfig == null)
                throw new NullReferenceException();


            DesignConfig_XmlData designConfigXml = designConfig.Design_XmlConfig;

            var result = new
            {
                entity_KeyName = "Design_Config_ID",
                entity_Id = designConfig.Design_Config_ID,
                Design_Config_ID = designConfig.Design_Config_ID,
                text = designConfig.Name,
                expanded = true,
                description = designConfig.Description,
                id = "Config",
                type = "Config",
                children = new List<dynamic>
                        {
                            new
                            {
                              text = "HomePage",
                              Design_Config_ID =  designConfig.Design_Config_ID,
                              entity_Id = "HomePage",
                              expanded =  true,
                              type = "HomePage",
                              id = "HomePage",
                              hierarchyDegree = 0,
                              children = getHomePageChildren(designConfigXml, designConfig.Design_Config_ID),
                            },
                            new {
                              text = "Header",
                              Design_Config_ID =  designConfig.Design_Config_ID,
                              entity_Id = "HeaderRoot",
                              expanded =  true,
                              type = "HeaderRoot",
                              id = "HeaderRoot",
                              hierarchyDegree = 0,
                              children =  getHeadersChildren(designConfigXml, designConfig.Design_Config_ID),
                            }
                        }
            };

            return Json(new
            {
                data = result
            });

        }

        private object getHeadersChildren(DesignConfig_XmlData designConfigXml, int Design_Config_ID)
        {
            if (designConfigXml == null)
                throw new NullReferenceException();

            var result = designConfigXml.Headers.Select(item => new
            {
                item.ParentId,
                Design_Config_ID = Design_Config_ID,
                entity_Id = item.MenuItemId,
                item.MenuItemId,
                id = item.MenuItemId,
                text = item.Label,
                leaf = item.SubMenu_List.Count() > 0 ? false : true,
                allowSubMenuItem = true,
                type = "MenuItem",
                expanded = false,
                hierarchyDegree = 1,
                childrenCount = item.SubMenu_List.Count(),
                children = item.SubMenu_List.Select(subItem => new
                {
                    Design_Config_ID = Design_Config_ID,
                    entity_Id = item.MenuItemId,
                    subItem.ParentId,
                    subItem.MenuItemId,
                    id = subItem.MenuItemId,
                    text = subItem.Label,
                    allowSubMenuItem = false,
                    leaf = true,
                    type = "MenuItem",
                    expanded = false,
                    hierarchyDegree = 2,
                    childrenCount = subItem.SubMenu_List.Count()
                })
            }).ToList();

            return result;
        }

        private object getHomePageChildren(DesignConfig_XmlData designConfigXml, int Design_Config_ID)
        {
            if (designConfigXml == null)
                throw new NullReferenceException();

            var homePageConfig = designConfigXml.HomePage;

            var result = new
            {
                text = "Carroussel Images",
                Design_Config_ID = Design_Config_ID,
                entity_Id = "CarouselImagesRoot",
                expanded = true,
                type = "CarouselImagesRoot",
                id = "CarouselImagesRoot",
                hierarchyDegree = 1,
                children = homePageConfig?.CarousselImage_Set.Select((subItem, i) => new
                {
                    Design_Config_ID = Design_Config_ID,
                    entity_Id = subItem.CarouselImage_ID,
                    text = subItem.CarouselImage_Name,
                    PositionItem = i,
                    leaf = true,
                    type = "CarouselImage",
                    expanded = false,
                    hierarchyDegree = 2
                })
            };

            return result;
        }


        public ActionResult updateActionEngineConfig(int Design_Config_ID, string entity_KeyName, string propertyName, string itemId, string value, bool? checkedValue)
        {
            var engine = design_Repository.FindAllDesign_Config()
                                          .Where(item => item.Design_Config_ID == Design_Config_ID)
                                          .FirstOrDefault();

            if (engine == null)
                throw new Exception("Design_Config_ID se semble pas exister.");

            if (entity_KeyName == "MenuItem")
            {
                engine.updateMenuItemProperty(itemId, propertyName, value, checkedValue);
            }
            else
            {
                throw new Exception(" missing or unknown entityKeyType !");
            }


            design_Repository.Save();

            return Json(new
            {
                msg = "Edité avec succès",
                success = true
            });
        }


        [DirectIgnore]
        public void createXmlConfig()
        {
            var config = design_Repository.FindAllDesign_Config().FirstOrDefault();
            DesignConfig_XmlData xml = new DesignConfig_XmlData();

            #region Datas
            xml.Headers.Add(new TwoGridProductMenuItem
            {
                Label = "Tout voir",
                ParentId = null,
                MenuItemId = "0",

                gridView1 = new GridView
                {
                    Products_Flags_Set = new List<Product_Flags>
                                {
                                    Product_Flags.Nouveautes
                                }
                },
                gridView2 = new GridView
                {

                }

            });

            xml.Headers.Add(new OneGridProductMenuItem
            {
                ParentId = null,
                MenuItemId = "1",
                Label = "Ma Jolie Brocante",
                SubMenu_List = new List<MenuItem>
                {
                    new OneGridProductMenuItem {
                        ParentId = "1",
                        MenuItemId = "1,0",
                        Label ="Electric Vintage",
                        gridView1 = new GridView
                        {
                            Category_ID_Set = new List<int>
                            {
                                17
                            }
                        }
                    },
                    new OneGridProductMenuItem {
                        ParentId = "1",
                        MenuItemId = "1,1",
                        Label ="Vaisselle",
                        gridView1 = new GridView
                        {
                            Category_ID_Set = new List<int>
                            {
                                4
                            }
                        }
                    },
                    new OneGridProductMenuItem {
                        Label ="Boite",
                        ParentId = "1",
                        MenuItemId = "1,2",
                    },
                    new OneGridProductMenuItem {
                        Label ="Vase",
                        ParentId = "1",
                        MenuItemId = "1,3",
                        gridView1 = new GridView
                        {
                            Category_ID_Set = new List<int>
                            {
                                6
                            }
                        }
                    },
                    new OneGridProductMenuItem {
                        Label ="Insolite",
                        ParentId = "1",
                        MenuItemId = "1,4",
                        gridView1 = new GridView
                        {
                            Category_ID_Set = new List<int>
                            {
                                8
                            }
                        }
                    }
                }
            });

            xml.Headers.Add(new OneGridProductMenuItem
            {
                Label = "Maison",
                ParentId = null,
                MenuItemId = "2",
                SubMenu_List = new List<MenuItem>
                {
                    new OneGridProductMenuItem {
                        Label ="Objet Déco",
                        ParentId = "2",
                        MenuItemId = "2,0",
                        gridView1 = new GridView
                        {
                            Category_ID_Set = new List<int>
                            {
                                9
                            }
                        }
                    },
                    new OneGridProductMenuItem {
                        Label ="Cuisine",
                        ParentId = "2",
                        MenuItemId = "2,1",
                        gridView1 = new GridView
                        {
                            Category_ID_Set = new List<int>
                            {
                                10
                            }
                        }
                    }
                }
            });
            xml.Headers.Add(new OneGridProductMenuItem
            {
                Label = "Papeterie",
                ParentId = null,
                MenuItemId = "3",
                gridView1 = new GridView
                {
                }
            });

            xml.Headers.Add(new OneGridProductMenuItem
            {
                ParentId = null,
                MenuItemId = "4",
                Label = "Créateurs",
                gridView1 = new GridView
                {
                }
            });

            xml.Headers.Add(new OneGridProductMenuItem
            {
                ParentId = null,
                MenuItemId = "5",
                Label = "TotoQuoi",
                SubMenu_List = new List<MenuItem>
                {
                    new OneGridProductMenuItem {
                        Label ="TotoQuoiFrere",
                        ParentId = "5",
                        MenuItemId = "5,0",
                        gridView1 = new GridView
                        {
                            Category_ID_Set = new List<int>
                            {
                                9
                            }
                        }
                    },
                    new OneGridProductMenuItem {
                        Label ="TotoQuoiFrere2",
                        ParentId = "5",
                        MenuItemId = "5,1",
                        gridView1 = new GridView
                        {
                            Category_ID_Set = new List<int>
                            {
                                10
                            }
                        }
                    }
                }
            });
            #endregion

            config.xmlData = xml.Serialize();
            design_Repository.Save();

        }

        public ActionResult getDesignMenuItemProperty_Set(DirectStoreQuery q)
        {
            Design_Config designConfig = null;
            IEnumerable<Parametrizer_PropertyManager> properties;
            //var activeConfig = design_Repository.getActiveDesignConfig();


            if (q.filterHasProperty("Design_Config_ID"))
            {
                int designCongif_Id = Int32.Parse(q.filterValue("Design_Config_ID"));
                designConfig = design_Repository.FindAllDesign_Config().Where(item => item.Design_Config_ID == designCongif_Id).FirstOrDefault();
            }

            if (q.filterHasProperty("MenuItem"))
            {
                string menuItemId = q.filterValue("MenuItem");
                var menuItem = designConfig.FindHeaderMenuItem(menuItemId);
                properties = menuItem.Property_Set;
            }
            else
            {
                throw new Exception(" missing MenuItem parameter!");
            }


            var result = properties?.Where(item => item.Enabled == true).Select(item => new
            {
                item.Name,
                Value = item.ValueDisplay,
                Description = item.Description,
                AvailableValuesChecked = item.SelectedValues,
                AvailableValues = item.Parameters.AvailableValues.Select(p => new
                {
                    valueField = p,
                    displayField = p
                }),
                FieldType = item.Parameters.FieldType.ToString()
            });

            return Json(new
            {
                data = result,
                total = result?.Count(),
                success = true
            });

        }

        public ActionResult getMenuItem_Set()
        {
            List<string> listMenuItemType = new List<string>
            {
                typeof(OneGridProductMenuItem).Name,
                typeof(TwoGridProductMenuItem).Name,
                typeof (UrlMenuItem).Name
            };

            return Json(new
            {
                data = listMenuItemType.Select(item => new { Name = item, nodeTargetForDrop = new string[] { "HeaderRoot", typeof(MenuItem).Name }, Type = typeof(MenuItem).Name })
                    .OrderBy(e => e.Name),
                total = listMenuItemType.Count()
            });
        }

        public ActionResult addItemToDesignConfig(int DesignConfig_ID, string targetItemType, string targetItemId, string itemTypeToAdd, string itemId = null)
        {
            var designConfig = design_Repository.FindAllDesign_Config().Where(item => item.Design_Config_ID == DesignConfig_ID).FirstOrDefault();


            if (designConfig == null)
            {
                throw new Exception("Design_Config_ID se semble pas exister.");
            }

            if (typeof(MenuItem).Name == targetItemType || "HeaderRoot" == targetItemType)
            {
                designConfig.AddMenuItem(targetItemId, itemTypeToAdd, targetItemType);
            }

            if ("CarouselImagesRoot" == targetItemType)
            {
                var carouselId = -1;
                var isParseOk = Int32.TryParse(itemId, out carouselId);
                var carouselImage = design_Repository.FindCarouselImage(carouselId);

                if (carouselImage == null)
                {
                    return Json(new
                    {
                        msg = "Le Carroussel Image ne semble plus exister",
                        success = false
                    });
                }

                designConfig.AddCarouselImage(carouselId);
            }
            design_Repository.Save();

            return Json(new
            {
                msg = "La configuration Design a bien été sauvegardée avec succès",
                success = true
            });
        }

        public ActionResult removeItemFromDesignConfig(int Design_Config_ID, string targetItemType, string itemId, int? positionItem = null)
        {
            var designConfig = design_Repository.FindAllDesign_Config().Where(item => item.Design_Config_ID == Design_Config_ID).FirstOrDefault();

            if (designConfig == null)
            {
                throw new Exception("Design_Config_ID se semble pas exister.");
            }

            if (typeof(MenuItem).Name == targetItemType)
            {
                designConfig.RemoveMenuItem(itemId);
            }
            else if (typeof(CarouselImage).Name == targetItemType)
            {
                var position = positionItem == null ? -1 : (int)positionItem;
                designConfig.RemoveCarouselImage(itemId, position);
            }
            this.design_Repository.Save();

            return Json(new
            {
                msg = "La configuration Design a bien été sauvegardée avec succès",
                success = true
            });
        }

        [FormHandler]
        public ActionResult createDesignConfig(string Name, string Description = null)
        {
            this.design_Repository.createDesignConfig(Name, Description);

            this.design_Repository.Save();

            return Json(new
            {
                msg = "Nouveau Design Config créé avec succès",
                success = true
            });
        }

        public ActionResult activateDesignConfig(int Design_Config_ID)
        {
            var designConfig = design_Repository.FindAllDesign_Config().Where(item => item.Design_Config_ID == Design_Config_ID).FirstOrDefault();

            if (designConfig == null)
            {
                return Json(new
                {
                    msg = ("La configuration Design " + Design_Config_ID + " se semble plus exister."),
                    success = false
                });
            }


            if (!designConfig.CurrentUserAbility.canActiveDesignConfig)
            {
                return Json(new
                {
                    msg = "Vous ne pouvez pas activer cette configuration Design",
                    success = false
                });
            }

            this.design_Repository.getActiveDesignConfig().Status = Design_Config_Status.Disabled;
            designConfig.Status = Design_Config_Status.Active;

            this.design_Repository.Save();
            return Json(new
            {
                msg = "La configuration Design a bien été activée avec succès",
                success = true
            });
        }

        public ActionResult deleteDesignConfig(int Design_Config_ID)
        {
            var designConfig = design_Repository.FindAllDesign_Config().Where(item => item.Design_Config_ID == Design_Config_ID).FirstOrDefault();

            if (designConfig == null)
            {
                return Json(new
                {
                    msg = ("La configuration Design " + Design_Config_ID + " se semble plus exister."),
                    success = false
                });
            }

            if (!designConfig.CurrentUserAbility.canDeleteDesignConfig)
            {
                return Json(new
                {
                    msg = "Vous ne pouvez pas supprimer cette configuration Design",
                    success = false
                });
            }

            design_Repository.deleteDesignCOnfig(designConfig);
            design_Repository.Save();

            return Json(new
            {
                msg = "La configuration Design a bien été supprimée avec succès",
                success = true
            });
        }

        [FormHandler]
        public ActionResult changePositionItem(int Design_Config_ID, int newIndexPosition, string targetItemType, string itemId, int? PositionItem = null)
        {
            var designConfig = design_Repository.FindAllDesign_Config().Where(item => item.Design_Config_ID == Design_Config_ID).FirstOrDefault();
            bool isChangePositionSuccess = false;

            if (designConfig == null)
            {
                throw new Exception("Design_Config_ID se semble pas exister.");
            }


            if (typeof(MenuItem).Name == targetItemType)
            {
                isChangePositionSuccess = designConfig.ChangeMenuItemPosition(itemId, targetItemType, newIndexPosition);

            }
            else if (typeof(CarouselImage).Name == targetItemType)
            {
                isChangePositionSuccess = designConfig.ChangeCarouselImagePosition(itemId, targetItemType, newIndexPosition, PositionItem);
            }

            if (!isChangePositionSuccess)
            {
                return Json(new
                {
                    msg = "La position n'a pas pu être changée, vérifiez bien que la position soit bonne",
                    success = false
                });
            }

            design_Repository.Save();

            return Json(new
            {
                msg = "La configuration Design a bien été sauvegardée avec succès",
                success = true
            });
        }


        [FormHandler]
        public ActionResult deleteEtiquette(int Etiquette_ID)
        {
            var etiquette = this.design_Repository.FindEtiquette(Etiquette_ID);

            if (etiquette == null)
            {
                return (ActionResult)this.Json((object)new
                {
                    msg = "L'Etiquette recherchée ne sembe plus exister",
                    success = false
                });
            }
            if (etiquette.isUseSomewhere)
            {
                return Json(new
                {
                    msg = ("<div style=\"text-align:left;\">L'étiquette " + etiquette.Etiquette_Name + " (Etiquette_ID: " + etiquette.Etiquette_ID + ") ne peut pas être supprimé, car elle est utilisée par d'autres données. </br></br> Endroits éventuels:<ul><li>Design Configuration</li><li>Categorie</li></ul></div>"),
                    success = false
                });
            }

            design_Repository.deleteEtiquette(etiquette);
            design_Repository.Save();

            return Json(new
            {
                msg = "L'Etiquette a bien été supprimée avec succès",
                success = true
            });
        }

        public ActionResult getEtiquette_Set()
        {
            var allEtiquette = design_Repository.FindAllEtiquette();


            var list = allEtiquette.Select(item => new
            {
                Etiquette_ID = item.Etiquette_ID,
                Etiquette_Name = item.Etiquette_Name
            }).ToList();


            return (ActionResult)this.Json((object)new
            {
                success = true,
                data = list
            });
        }

        [FormHandler]
        public ActionResult createEtiquette(string Etiquette_Name)
        {
            HttpFileCollectionBase files = Request.Files;

            #region Ontology
            if (Request.Files.Count < 1)
            {
                throw new Exception("Aucun fichier n'a été soumis !");
            }

            if (Request.Files.Count > 1)
            {
                throw new Exception("Plus de 1 ficheir a été soumis !");
            }

            HttpPostedFileBase file = this.Request.Files[0];
            if (file.ContentLength == 0)
            {
                throw new Exception("Le fichier est-il vide ??");
            }

            if (file.ContentLength > 10000000)
            {
                return Json(new
                {
                    msg = "Le fichier est trop large, il doit être en dessous de 10MB",
                    success = false
                });
            }

            if (Etiquette_Name == null)
            {
                return Json(new
                {
                    msg = "Etiquette_Name n'pas été soumis",
                    success = false
                });
            }

            if (design_Repository.FindEtiquetteByName(Etiquette_Name) != null)
            {
                return Json(new
                {
                    msg = "Une Etiquette portant le nom '" + Etiquette_Name + "' existe déjà",
                    success = false
                });
            }
            #endregion

            byte[] buffer = new byte[file.ContentLength];
            file.InputStream.Read(buffer, 0, file.ContentLength);

            Blob blob = new Blob
            {
                CreationDateTime = new DateTime?(DateTime.Now),
                FileName = file.FileName,
                MimeType = file.ContentType,
                Stream = buffer
            };


            design_Repository.addEtiquette(new Etiquette()
            {
                Etiquette_Name = Etiquette_Name,
                Blob = blob
            });

            this.design_Repository.Save();

            return Json(new
            {
                msg = "Etiquette créé avec succès",
                success = true
            });
        }

        [FormHandler]
        public ActionResult createCarouselImage(string CarouselImage_Name)
        {
            HttpFileCollectionBase files = Request.Files;

            #region Ontology
            if (Request.Files.Count < 1)
            {
                throw new Exception("Aucun fichier n'a été soumis !");
            }

            if (Request.Files.Count > 1)
            {
                throw new Exception("Plus de 1 ficheir a été soumis !");
            }

            HttpPostedFileBase file = this.Request.Files[0];
            if (file.ContentLength == 0)
            {
                throw new Exception("Le fichier est-il vide ??");
            }

            if (file.ContentLength > 10000000)
            {
                return Json(new
                {
                    msg = "Le fichier est trop large, il doit être en dessous de 10MB",
                    success = false
                });
            }

            if (CarouselImage_Name == null)
            {
                return Json(new
                {
                    msg = "CarouselImage_Name n'pas été soumis",
                    success = false
                });
            }

            if (design_Repository.FindCarouselImageByName(CarouselImage_Name) != null)
            {
                return Json(new
                {
                    msg = "Un Carroussel Image portant le nom '" + CarouselImage_Name + "' existe déjà",
                    success = false
                });
            }
            #endregion

            byte[] buffer = new byte[file.ContentLength];
            file.InputStream.Read(buffer, 0, file.ContentLength);

            Blob blob = new Blob
            {
                CreationDateTime = new DateTime?(DateTime.Now),
                FileName = file.FileName,
                MimeType = file.ContentType,
                Stream = buffer
            };


            design_Repository.addCarouselImage(new CarouselImage
            {
                CarouselImage_Name = CarouselImage_Name,
                Blob = blob
            });

            this.design_Repository.Save();

            return Json(new
            {
                msg = "Carroussel Image créé avec succès",
                success = true
            });
        }

        [FormHandler]
        public ActionResult deleteCarouselImage(int CarouselImage_ID)
        {
            var carouselImage = this.design_Repository.FindCarouselImage(CarouselImage_ID);

            if (carouselImage == null)
            {
                return (ActionResult)this.Json((object)new
                {
                    msg = "Le Carroussel Image recherché ne sembe plus exister",
                    success = false
                });
            }
            if (carouselImage.isUseSomewhere)
            {
                return Json(new
                {
                    msg = ("<div style=\"text-align:left;\">Le carroussel " + carouselImage.CarouselImage_ID + " (CarouselImage_ID: " + carouselImage.CarouselImage_Name + ") ne peut pas être supprimé, car il est utilisé par d'autres données. </br></br> Endroits éventuels:<ul><li>Design Configuration</li><li>Carroussel Images</li></ul></div>"),
                    success = false
                });
            }

            design_Repository.deleteCarouselImage(carouselImage);
            design_Repository.Save();

            return Json(new
            {
                msg = "Le CarouselImage a bien été supprimé avec succès",
                success = true
            });
        }
        public ActionResult getCarouselImage_Set()
        {
            var allCarouselImage = design_Repository.FindAllCarouselImage();


            var list = allCarouselImage.AsEnumerable().Select(item => new
            {
                Name = item.CarouselImage_Name,
                CarouselImage_ID = item.CarouselImage_ID,
                CarouselImage_Name = item.CarouselImage_Name,
                nodeTargetForDrop = new string[] { "CarouselImagesRoot", typeof(CarouselImage).Name },
                Type = typeof(CarouselImage).Name
            }).ToList();


            return (ActionResult)this.Json((object)new
            {
                success = true,
                data = list
            });
        }


    }
}