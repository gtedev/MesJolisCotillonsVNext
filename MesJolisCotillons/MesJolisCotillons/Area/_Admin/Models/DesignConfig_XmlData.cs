using MesJolisCotillons.Extensions.XmlData;
using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using static MesJolisCotillons.Area._Admin.Parametrizer_PropertyManager;
using static ProductsPageRequest;

namespace MesJolisCotillons.Area._Admin
{
    //[XmlInclude(typeof(HomePageConfig))]
    public class DesignConfig_XmlData : XmlSerializer<DesignConfig_XmlData>
    {
        public List<MenuItem> Headers { get; set; } = new List<MenuItem>();
        public HomePageConfig HomePage { get; set; }


        public MenuItem FindHeaderMenuItem(string menuItemId)
        {
            MenuItem resultMenuItem = null;

            if (menuItemId == null)
                return resultMenuItem;

            List<string> listSplitted = menuItemId.Split(',').ToList();
            if (listSplitted.Count() == 0)
                return resultMenuItem;

            #region Parsing splitted string list to int list
            List<int> listIdsParsed = new List<int>();
            foreach (string s in listSplitted)
            {
                int result;
                if (!int.TryParse(s, out result))
                {
                    return resultMenuItem;
                }
                listIdsParsed.Add(result);
            }
            #endregion

            int indexFirstLevel = listIdsParsed.FirstOrDefault();
            resultMenuItem = Headers.ElementAtOrDefault(indexFirstLevel);

            if (resultMenuItem == null)
                return resultMenuItem;

            for (int i = 1; i < listIdsParsed.Count(); i++)
            {
                if (resultMenuItem != null)
                {
                    int subLevelIndex = listIdsParsed[i];
                    resultMenuItem = resultMenuItem.SubMenu_List.ElementAtOrDefault(subLevelIndex);
                }
            }
            return resultMenuItem;
        }

        public void AddMenuItemToHeader(MenuItem menuItem)
        {
            menuItem.ParentId = null;
            menuItem.MenuItemId = this.Headers.Count().ToString();
            menuItem.Label = "-no Name-";
            this.Headers.Add(menuItem);
            this.recomputeAllMenuItemId();
        }

        public void RemoveMenuItem(string menuItemId)
        {
            MenuItem menuItem = this.FindHeaderMenuItem(menuItemId);

            if (menuItem == null)
            {
                throw new Exception("RemoveMenuItem: MenuItem " + menuItemId + " introuvable !");
            }

            MenuItem parentMenuItem = this.FindHeaderMenuItem(menuItem.ParentId);
            if (parentMenuItem == null)
            {
                this.Headers.Remove(menuItem);
            }

            else
            {
                parentMenuItem.SubMenu_List.Remove(menuItem);
            }

            this.recomputeAllMenuItemId();
        }
        public void RemoveCarouselImage(int carrouselId, int positionItem)
        {
            var carrouselIdResult = this.HomePage?.CarousselImage_ID_Set.ElementAt(positionItem);

            if (carrouselIdResult == null || carrouselIdResult != carrouselId)
            {
                throw new Exception("RemoveMenuItem: carrouselIdResult " + carrouselId + " introuvable !");
            }

            this.HomePage?.CarousselImage_ID_Set.RemoveAt(positionItem);
        }

        public bool ChangeMenuItemPosition(string itemId, string targetItemType, int newIndexPosition)
        {
            int index = newIndexPosition - 1;
            MenuItem menuItem = this.FindHeaderMenuItem(itemId);
            MenuItem parentMenuItem = this.FindHeaderMenuItem(menuItem.ParentId);

            if (parentMenuItem == null)
            {
                if (index > this.Headers.Count() - 1 || index < 0)
                {
                    return false;
                }

                this.Headers.Remove(menuItem);
                this.Headers.Insert(index, menuItem);
            }
            else
            {
                if (index > parentMenuItem.SubMenu_List.Count() - 1 && index < 0)
                {
                    return false;
                }

                parentMenuItem.SubMenu_List.Remove(menuItem);
                parentMenuItem.SubMenu_List.Insert(index, menuItem);
            }
            this.recomputeAllMenuItemId();
            return true;
        }

        public bool ChangeCarouselImagePosition(string itemId, string targetItemType, int newIndexPosition, int? PositionItem)
        {
            int index = newIndexPosition - 1;
            var castedPositionItem = -1;
            var parsedItemId = -1;
            var isParseOk = Int32.TryParse(itemId, out parsedItemId);

            if (PositionItem != null)
            {
                castedPositionItem = (int)PositionItem;
            }
            var targetCarouselId = this.HomePage?.CarousselImage_ID_Set.ElementAtOrDefault(castedPositionItem);


            if (targetCarouselId != null && targetCarouselId == parsedItemId)
            {
                if (index > this.HomePage?.CarousselImage_ID_Set.Count - 1 || index < 0)
                {
                    return false;
                }

                this.HomePage?.CarousselImage_ID_Set.RemoveAt(castedPositionItem);
                this.HomePage?.CarousselImage_ID_Set.Insert(index, (int)targetCarouselId);
            }
            return true;
        }
        private void recomputeAllMenuItemId()
        {
            if (this.Headers == null)
                return;

            for (int index = 0; index < this.Headers.Count(); index++)
            {
                MenuItem header = this.Headers[index];
                header.ParentId = null;
                string itemIdString = index.ToString();
                header.MenuItemId = itemIdString;
                header.recomputeAllSubMenuItemId();
            }
        }

        public bool isCategoryUsedInConfig(Category category)
        {
            bool flag = false;
            foreach (MenuItem header in this.Headers)
            {
                ICategoryChecking categoryChecking = header as ICategoryChecking;
                if (categoryChecking != null && categoryChecking.isCategoryUsed(category))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public bool isKeyWordUsedInConfig(KeyWord keyWord)
        {
            bool flag = false;
            foreach (MenuItem header in this.Headers)
            {
                IKeyWordChecking keyWordChecking = header as IKeyWordChecking;
                if (keyWordChecking != null && keyWordChecking.isKeyWordUsed(keyWord))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public bool isEtiquetteUsedInConfig(Etiquette etiquette)
        {
            bool flag = false;
            foreach (MenuItem header in this.Headers)
            {
                IEtiquetteChecking etiquetteChecking = header as IEtiquetteChecking;
                if (etiquetteChecking != null && etiquetteChecking.isEtiquetteUsed(etiquette))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        public bool isCarouselImageUsedInConfig(CarouselImage carouselImage)
        {
            var carouselImage_ID_Set = this.HomePage?.CarousselImage_ID_Set;

            if (carouselImage_ID_Set == null)
            {
                return false;
            }

            return carouselImage_ID_Set.Any(id => id == carouselImage.CarouselImage_ID);
        }
    }

    [XmlInclude(typeof(OneGridProductMenuItem))]
    [XmlInclude(typeof(TwoGridProductMenuItem))]
    [XmlInclude(typeof(UrlMenuItem))]
    public class MenuItem : ParametrizerEngine
    {
        public string ParentId { get; set; }
        public string MenuItemId { get; set; }
        public string Label { get; set; }
        public bool EnableClickFilter { get; set; } = true;

        public List<MenuItem> SubMenu_List { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager LabelProperty
        {
            get
            {
                var Parameters = new Parametrizer_PropertyManager.Parametrizer_Fields
                {
                    FieldType = Parametrizer_PropertyManager.Parametrizer_FieldType.Text,
                    AvailableValues = new string[] { true.ToString(), false.ToString() }.ToList()
                };
                return new Parametrizer_PropertyManager
                {
                    Name = "Label",
                    ValueDisplay = this.Label,
                    Description = "Changer cette proprieté pour changer le text qui sera affiché sur le site pour le boutton menu",
                    updateProperty = (value, checkedValue) =>
                    {
                        this.Label = value;
                    },
                    Parameters = Parameters

                };
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager EnableClickFilterProperty
        {
            get
            {
                var Parameters = new Parametrizer_PropertyManager.Parametrizer_Fields
                {
                    FieldType = Parametrizer_PropertyManager.Parametrizer_FieldType.SingleSelect,
                    AvailableValues = new string[] { true.ToString(), false.ToString() }.ToList()
                };
                return new Parametrizer_PropertyManager
                {
                    Name = "ActiverFiltreOnClick",
                    ValueDisplay = this.EnableClickFilter.ToString(),
                    Description = "True pour activer le filtrage des produits sur l'action du clique.",
                    updateProperty = (value, checkedValue) =>
                    {
                        this.EnableClickFilter = Boolean.Parse(value);
                    },
                    Parameters = Parameters

                };
            }
        }

        public void AddSubMenuItem(MenuItem subMenuItem)
        {
            subMenuItem.ParentId = this.MenuItemId;
            subMenuItem.MenuItemId = this.MenuItemId + "," + this.SubMenu_List.Count();
            subMenuItem.Label = "-no Name-";
            this.SubMenu_List.Add(subMenuItem);
            this.recomputeAllSubMenuItemId();
        }

        public void recomputeAllSubMenuItemId()
        {
            if (this.SubMenu_List == null)
                return;

            for (int index = 0; index < this.SubMenu_List.Count(); index++)
            {
                this.SubMenu_List[index].MenuItemId = this.MenuItemId + "," + index.ToString();
            }
        }
    }

    public class OneGridProductMenuItem : MenuItem, ICategoryChecking, IKeyWordChecking, IEtiquetteChecking, IProductViewModel
    {

        public GridView gridView1 { get; set; } = new GridView();
        private Design_Repository design_Repository = new Design_Repository();

        #region Properties

        protected Product_Repository product_Repository = new Product_Repository();

        protected List<Category> gridViewCategory_List(GridView grid)
        {
            List<Category> categoryList = new List<Category>();
            foreach (int categoryId in grid.Category_ID_Set)
            {
                Category category = this.product_Repository.FindCategory(categoryId);
                if (category != null)
                {
                    categoryList.Add(category);
                }
            }
            return categoryList;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager gridView1CategoryProperty
        {
            get
            {
                var categoryList = product_Repository.FindAllCategory();

                var Parameters = new Parametrizer_PropertyManager.Parametrizer_Fields
                {
                    FieldType = Parametrizer_PropertyManager.Parametrizer_FieldType.MultiSelect,
                    AvailableValues = categoryList.Select(e => e.Name).ToList()
                };

                var categoryProperty = new Parametrizer_PropertyManager
                {
                    Parameters = Parameters,
                    Description = "Ajouter des catégories pour filtrer les produits qui seront affichés lorsque le bouton menu sera cliqué",
                    Name = "GridView1_Categories",
                    ValueDisplay = gridView1.Category_ID_Set.Count > 0 ? gridViewCategory_List(gridView1).Select(item => item.Name.ToString()).Aggregate((a, b) => a + ", " + b) : "",
                    SelectedValues = Parameters.AvailableValues.Select(item => new
                    {
                        Value = item.ToString(),
                        Checked = gridViewCategory_List(gridView1).Any(c => c.Name.Equals(item)) ? true : false,
                        Value_Int = item
                    }),
                    updateProperty = (value, checkedValue) =>
                    {
                        Category category = categoryList.Where(c => c.Name == value).FirstOrDefault();

                        if ((bool)checkedValue)
                        {
                            gridView1.Category_ID_Set.Add(category.Category_ID);
                        }
                        else
                        {
                            gridView1.Category_ID_Set.Remove(category.Category_ID);
                        }
                    }
                };
                return categoryProperty;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager gridView1_KeyWordProperty
        {
            get
            {
                IQueryable<KeyWord> keyWordList = this.product_Repository.FindAllKeyWord();

                var Parameters = new Parametrizer_Fields
                {
                    FieldType = Parametrizer_FieldType.MultiSelect,
                    AvailableValues = keyWordList.Select(e => e.Value).ToList()
                };


                return new Parametrizer_PropertyManager
                {
                    Name = "GridView1_MotCles",
                    ValueDisplay = this.gridView1.KeyWord_Set.Count > 0 ? this.gridView1.KeyWord_Set.Aggregate((a, b) => a + ", " + b) : "",
                    Description = "Ajouter des mots clés pour filtrer les produits qui seront affichés lorsque le bouton menu sera cliqué",
                    SelectedValues = Parameters.AvailableValues.Select(item => new
                    {
                        Value = item.ToString(),
                        Checked = this.gridView1.KeyWord_Set.Any(k => k.Equals(item)),
                        Value_Int = item
                    }),
                    updateProperty = (value, checkedValue) =>
                    {
                        KeyWord keyWord = keyWordList.Where(c => c.Value == value).FirstOrDefault();
                        if (checkedValue.Value)
                        {
                            this.gridView1.KeyWord_Set.Add(keyWord.Value);
                        }

                        else
                        {
                            this.gridView1.KeyWord_Set.Remove(keyWord.Value);
                        }

                    },
                    Parameters = Parameters
                };
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager productsPerPageProperty
        {
            get
            {
                var Parameters = new Parametrizer_PropertyManager.Parametrizer_Fields
                {
                    FieldType = Parametrizer_PropertyManager.Parametrizer_FieldType.Number,
                    AvailableValues = new string[] { true.ToString(), false.ToString() }.ToList()
                };

                return new Parametrizer_PropertyManager
                {
                    Name = "Nombre_de_Produits_par_Page",
                    ValueDisplay = this.gridView1.numberOfProductsPerPage,
                    Description = "Configurez le nombre de produits que vous voulez par page",
                    updateProperty = (value, checkedValue) =>
                    {
                        this.gridView1.numberOfProductsPerPage = int.Parse(value);
                    },
                    Parameters = Parameters
                };
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager EtiquetteProperty
        {
            get
            {
                List<string> list = this.design_Repository.FindAllEtiquette()
                                                          .OrderBy(item => item.Etiquette_Name)
                                                          .Select(item => item.Etiquette_Name)
                                                          .ToList();

                var Parameters = new Parametrizer_PropertyManager.Parametrizer_Fields
                {
                    FieldType = Parametrizer_PropertyManager.Parametrizer_FieldType.SingleSelect,
                    AvailableValues = list
                };

                Etiquette etiquette = this.design_Repository.FindEtiquette(this.gridView1.Etiquette_ID);

                return new Parametrizer_PropertyManager
                {
                    Name = "Etiquette",
                    ValueDisplay = etiquette == null ? "" : etiquette.Etiquette_Name,
                    Description = "Choisissez l'Etiquette que vous désirez afficher en tête de la Grid Produit",
                    updateProperty = (value, checkedValue) =>
                    {
                        Etiquette etiquetteByName = this.design_Repository.FindEtiquetteByName(value);
                        if (etiquetteByName == null)
                            return;
                        this.gridView1.Etiquette_ID = etiquetteByName.Etiquette_ID;
                    },
                    Parameters = Parameters
                };
            }
        }
        #endregion

        public virtual bool isCategoryUsed(Category category)
        {
            bool isCategoryUsed = this.gridView1.Category_ID_Set.Contains(category.Category_ID);
            if (isCategoryUsed || this.SubMenu_List.Count() == 0)
                return isCategoryUsed;

            foreach (MenuItem subMenu in this.SubMenu_List)
            {
                OneGridProductMenuItem oneMenuItem = subMenu as OneGridProductMenuItem;
                TwoGridProductMenuItem twoMenuItem = subMenu as TwoGridProductMenuItem;

                if (oneMenuItem != null)
                {
                    isCategoryUsed = oneMenuItem.isCategoryUsed(category);
                }
                else if (twoMenuItem != null)
                {
                    isCategoryUsed = twoMenuItem.isCategoryUsed(category);
                }

                if (isCategoryUsed)
                {
                    break;
                }
            }

            return isCategoryUsed;
        }

        public virtual bool isKeyWordUsed(KeyWord keyWord)
        {
            bool isKeyWordUsed = this.gridView1.KeyWord_Set.Contains(keyWord.Value);
            if (isKeyWordUsed)
            {
                return isKeyWordUsed;
            }
            if (this.SubMenu_List.Count<MenuItem>() > 0)
            {
                foreach (MenuItem subMenu in this.SubMenu_List)
                {
                    OneGridProductMenuItem oneMenuItem = subMenu as OneGridProductMenuItem;
                    TwoGridProductMenuItem twoMenuItem = subMenu as TwoGridProductMenuItem;

                    if (oneMenuItem != null)
                    {
                        isKeyWordUsed = oneMenuItem.isKeyWordUsed(keyWord);
                    }
                    else if (twoMenuItem != null)
                    {
                        isKeyWordUsed = twoMenuItem.isKeyWordUsed(keyWord);
                    }

                    if (isKeyWordUsed)
                    {
                        break;
                    }
                }
            }
            return isKeyWordUsed;
        }

        public virtual bool isEtiquetteUsed(Etiquette etiquette)
        {
            bool isEtiquetteUsed = this.gridView1.Etiquette_ID == etiquette.Etiquette_ID;
            if (isEtiquetteUsed)
            {
                return isEtiquetteUsed;
            }
            if (this.SubMenu_List.Count<MenuItem>() > 0)
            {
                foreach (MenuItem subMenu in this.SubMenu_List)
                {
                    OneGridProductMenuItem oneMenuItem = subMenu as OneGridProductMenuItem;
                    TwoGridProductMenuItem twoMenuItem = subMenu as TwoGridProductMenuItem;

                    if (oneMenuItem != null)
                    {
                        isEtiquetteUsed = oneMenuItem.isEtiquetteUsed(etiquette);
                    }
                    else if (twoMenuItem != null)
                    {
                        isEtiquetteUsed = twoMenuItem.isEtiquetteUsed(etiquette);
                    }

                    if (isEtiquetteUsed)
                    {
                        break;
                    }
                }
            }
            return isEtiquetteUsed;
        }

        public virtual ProductsViewModel getProductsViewModel()
        {
            List<QueryFilter> queryFilterList = new List<QueryFilter>();
            var design_Repository = new Design_Repository();

            #region Category filters
            foreach (int categoryId in this.gridView1.Category_ID_Set)
            {
                queryFilterList.Add(new QueryFilter
                {
                    property = "Category_ID",
                    value = categoryId.ToString()
                });
            }
            #endregion

            #region KeyWord filters
            foreach (string keyWord in this.gridView1.KeyWord_Set)
            {
                queryFilterList.Add(new QueryFilter()
                {
                    property = "KeyWord",
                    value = keyWord
                });
            }
            #endregion

            Etiquette etiquette = design_Repository.FindEtiquette(this.gridView1.Etiquette_ID);

            return new ProductsViewModel()
            {
                title = this.Label,
                Etiquette = etiquette,
                query = new ProductsPageRequest()
                {
                    pageNumber = 1,
                    filters = queryFilterList,
                    pageSize = this.gridView1.numberOfProductsPerPage
                }
            };
        }
    }
    public class TwoGridProductMenuItem : OneGridProductMenuItem
    {
        public GridView gridView2 { get; set; } = new GridView();

        #region Properties
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager gridView2CategoryProperty
        {
            get
            {
                var categoryList = product_Repository.FindAllCategory();

                var Parameters = new Parametrizer_PropertyManager.Parametrizer_Fields
                {
                    FieldType = Parametrizer_PropertyManager.Parametrizer_FieldType.MultiSelect,
                    AvailableValues = categoryList.Select(e => e.Name).ToList()
                };

                var categoryProperty = new Parametrizer_PropertyManager
                {
                    Parameters = Parameters,
                    Name = "GridView2_Categories",
                    Description = "Ajouter des catégories pour filtrer les produits qui seront affichés lorsque le bouton menu sera cliqué",
                    ValueDisplay = gridView2.Category_ID_Set.Count > 0 ? gridViewCategory_List(gridView2).Select(item => item.Name.ToString()).Aggregate((a, b) => a + ", " + b) : "",
                    SelectedValues = Parameters.AvailableValues.Select(item => new
                    {
                        Value = item.ToString(),
                        Checked = gridViewCategory_List(gridView2).Any(c => c.Name.Equals(item)) ? true : false,
                        Value_Int = item
                    }),
                    updateProperty = (value, checkedValue) =>
                    {
                        Category category = categoryList.Where(c => c.Name == value).FirstOrDefault();

                        if ((bool)checkedValue)
                        {
                            gridView2.Category_ID_Set.Add(category.Category_ID);
                        }
                        else
                        {
                            gridView2.Category_ID_Set.Remove(category.Category_ID);
                        }
                    }
                };
                return categoryProperty;
            }
        }




        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager gridView2_CategoryProperty
        {
            get
            {
                IQueryable<Category> categoryList = this.product_Repository.FindAllCategory();

                var Parameters = new Parametrizer_Fields
                {
                    FieldType = Parametrizer_FieldType.MultiSelect,
                    AvailableValues = categoryList.Select(e => e.Name).ToList()
                };


                return new Parametrizer_PropertyManager
                {
                    Name = "GridView2_Categories",
                    ValueDisplay = this.gridView2.Category_ID_Set.Count > 0 ? this.gridViewCategory_List(this.gridView2).Select(item => item.Name.ToString()).Aggregate((a, b) => a + ", " + b) : "",
                    Description = "Ajouter des catégories pour filtrer les produits qui seront affichés lorsque le bouton menu sera cliqué",
                    SelectedValues = Parameters.AvailableValues.Select(item => new
                    {
                        Value = item.ToString(),
                        Checked = this.gridViewCategory_List(this.gridView2).Any(c => c.Name.Equals(item)),
                        Value_Int = item
                    }),
                    updateProperty = (value, checkedValue) =>
                    {
                        Category category = categoryList.Where(c => c.Name == value).FirstOrDefault();
                        if (checkedValue.Value)
                        {
                            this.gridView2.Category_ID_Set.Add(category.Category_ID);
                        }

                        else
                        {
                            this.gridView2.Category_ID_Set.Remove(category.Category_ID);
                        }
                    },
                    Parameters = Parameters
                };
            }
        }
        #endregion

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager gridView2_KeyWordProperty
        {
            get
            {
                IQueryable<KeyWord> keyWordList = this.product_Repository.FindAllKeyWord();

                var Parameters = new Parametrizer_Fields
                {
                    FieldType = Parametrizer_FieldType.MultiSelect,
                    AvailableValues = keyWordList.Select(e => e.Value).ToList()
                };


                return new Parametrizer_PropertyManager
                {
                    Name = "GridView2_MotCles",
                    ValueDisplay = this.gridView2.KeyWord_Set.Count > 0 ? this.gridView2.KeyWord_Set.Aggregate((a, b) => a + ", " + b) : "",
                    Description = "Ajouter des mots clés pour filtrer les produits qui seront affichés lorsque le bouton menu sera cliqué",
                    SelectedValues = Parameters.AvailableValues.Select(item => new
                    {
                        Value = item.ToString(),
                        Checked = this.gridView2.KeyWord_Set.Any(k => k.Equals(item)),
                        Value_Int = item
                    }),
                    updateProperty = (value, checkedValue) =>
                    {
                        KeyWord keyWord = keyWordList.Where(c => c.Value == value).FirstOrDefault();
                        if (checkedValue.Value)
                        {
                            this.gridView2.KeyWord_Set.Add(keyWord.Value);
                        }

                        else
                        {
                            this.gridView2.KeyWord_Set.Remove(keyWord.Value);
                        }

                    },
                    Parameters = Parameters
                };
            }
        }

        public override bool isCategoryUsed(Category category)
        {

            return base.isCategoryUsed(category) && this.gridView2.Category_ID_Set.Contains(category.Category_ID);

        }

        public override bool isKeyWordUsed(KeyWord keyWord)
        {

            return base.isKeyWordUsed(keyWord) && this.gridView2.KeyWord_Set.Contains(keyWord.Value);
        }
    }
    public class GridView
    {
        public List<int> Category_ID_Set { get; set; } = new List<int>();

        public List<Product_Flags> Products_Flags_Set { get; set; } = new List<Product_Flags>();

        public List<string> KeyWord_Set { get; set; } = new List<string>();

        public int numberOfProductsPerPage { get; set; } = 21;

        public int Etiquette_ID { get; set; }
    }

    public enum ProductView_TemplateEnum : int
    {
        OneGridProductTemplate = 1,
        TwoGridProductTemplate = 2,
    }

    public class UrlMenuItem : MenuItem
    {
        public string Url { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [XmlIgnore]
        public Parametrizer_PropertyManager UrlProperty
        {
            get
            {

                var Parameters = new Parametrizer_PropertyManager.Parametrizer_Fields
                {
                    FieldType = Parametrizer_PropertyManager.Parametrizer_FieldType.Text,
                    AvailableValues = new string[] { true.ToString(), false.ToString() }.ToList()
                };

                return new Parametrizer_PropertyManager
                {
                    Name = "Url",
                    ValueDisplay = this.Url,
                    Description = "Indiquer l'URL vers lequel le site sera redirigé",
                    updateProperty = (value, checkedValue) =>
                    {
                        if (!String.IsNullOrEmpty(value))
                        {
                            this.Url = value.Trim();
                        }
                    },
                    Parameters = Parameters

                };
            }
        }
    }

    #region Parametrizer
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ParametrizerEngine
    {
        public ParametrizerEngine() { }
        public IEnumerable<Parametrizer_PropertyManager> Property_Set
        {
            get
            {
                var properties = this.GetType().GetProperties()
                    .Where(f => f.PropertyType == typeof(Parametrizer_PropertyManager))
                                    .Select(f => f.GetValue(this))
                .Cast<Parametrizer_PropertyManager>();
                return properties;
            }
        }
        public void updateActionProperty(string propertyName, string value, bool? checkedValue)
        {
            var properties = this.GetType().GetProperties()
                .Where(f => f.PropertyType == typeof(Parametrizer_PropertyManager))
                .Select(f => f.GetValue(this))
                .Cast<Parametrizer_PropertyManager>()
                .Where(ecp => ecp.Name == propertyName);
            foreach (Parametrizer_PropertyManager item in properties)
            {
                item.updateProperty(value, checkedValue);
            }
        }

        [JsonProperty]
        public string Description;
        [XmlIgnore]
        public Parametrizer_PropertyManager DescriptionProperty
        {
            get
            {
                var Parameters = new Parametrizer_PropertyManager.Parametrizer_Fields
                {
                    FieldType = Parametrizer_PropertyManager.Parametrizer_FieldType.Text,
                    AvailableValues = new string[] { true.ToString(), false.ToString() }.ToList()
                };
                return new Parametrizer_PropertyManager
                {
                    Name = "Description",
                    ValueDisplay = this.Description,
                    updateProperty = (value, checkedValue) =>
                    {
                        //this.Description = value;
                        if (!String.IsNullOrEmpty(value))
                        {
                            this.Description = value.Trim();
                        }
                    },
                    Parameters = Parameters

                };
            }
        }
    }
    public class Parametrizer_PropertyManager
    {
        public Parametrizer_PropertyManager()
        {
            updateProperty = (value, checkedValue) =>
            {
                throw new Exception("Parametrizer_PropertyManager Property " + this.Name + " does not have any updateProperty method implemented ! ");
            };
        }
        public Action<string, bool?> updateProperty { get; set; }
        public string Name { get; set; }
        public object ValueDisplay { get; set; }
        public object SelectedValues { get; set; }
        public Parametrizer_Fields Parameters { get; set; }
        public string Description { get; set; } = "...No description available...";
        public bool Enabled { get; set; } = true;
        public class Parametrizer_Fields
        {
            public Parametrizer_FieldType FieldType { get; set; }
            public List<string> AvailableValues { get; set; }
        }

        public enum Parametrizer_FieldType
        {
            SingleSelect = 1,
            MultiSelect = 2,
            Text = 3,
            Number = 4,
            Date = 5,
            Time = 6
        }

    }
    #endregion

    public interface ICategoryChecking
    {
        bool isCategoryUsed(Category category);
    }

    public interface IKeyWordChecking
    {
        bool isKeyWordUsed(KeyWord keyWord);
    }

    public interface IEtiquetteChecking
    {
        bool isEtiquetteUsed(Etiquette etiquette);
    }

    public interface IProductViewModel
    {
        ProductsViewModel getProductsViewModel();
    }

    public class HomePageConfig
    {
        public List<int> CarousselImage_ID_Set { get; set; } = new List<int>();

        [XmlIgnore]
        public List<CarouselImage> CarousselImage_Set
        {
            get
            {
                var repository = new Design_Repository();
                var resultList = new List<CarouselImage>();

                foreach (var carouselId in CarousselImage_ID_Set)
                {
                    var carouselImage = repository.FindCarouselImage(carouselId);
                    if (carouselImage != null)
                    {
                        resultList.Add(carouselImage);
                    }
                }

                return resultList;
            }
        }
    }
}