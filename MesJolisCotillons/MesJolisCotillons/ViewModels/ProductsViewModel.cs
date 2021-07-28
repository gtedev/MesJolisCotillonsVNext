using MesJolisCotillons.Area.Controllers;
using MesJolisCotillons.Extensions.Security;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesJolisCotillons.ViewModels
{
    public class ProductsViewModel
    {
        public string viewId { get; set; }
        public string title { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string imageLabelPath { get; set; }
        public ProductsPageRequest query { get; set; }
        public Etiquette Etiquette { get; set; }

        public string getProductViewModelId()
        {
            var viewId = "ProductsNoFilters";

            if (this?.query?.filters.Count() > 0)
            {
                viewId = this.query.filters.Select(item => item.property + item.value).Aggregate((a, b) => a + b);
            }

            viewId = CryptographyUtils.GenerateSHA256String(viewId).Substring(0, 10);

            return viewId;
        }
    }

    public class ProductsGridViewModel
    {
        public List<Product> products { get; set; }
        public PagingInfo pagingInfos { get; set; }
        public ProductsPageRequest query { get; set; }
    }

    public class PagingInfo
    {
        public int totalItems { get; set; }
        public int pageSize { get; set; }

        public int numberOfPages
        {
            get
            {
                int result = totalItems / pageSize;
                if ((double) totalItems % (double) pageSize > 0)
                {
                    result++;
                }

                return result;
            }
        }

        public int pageNumber { get; set; }
    }

    public class CategoryViewModel
    {
        public string title { get; set; }
        public string imageLabelPath { get; set; }
        public int Category_ID { get; set; } = -1;
        public Etiquette etiquette { get; set; }
    }
}

public class ProductsPageRequest
{
    public string ProductsViewId { get; set; }
    public int pageNumber { get; set; } = 1;
    public int pageSize { get; set; } = 20;

    public int start
    {
        get { return ((pageNumber - 1) * pageSize); }
    }

    public int last
    {
        get { return pageNumber * pageSize; }
    }

    public string productGridCls { get; set; }
    public bool scrollUpToHeader { get; set; } = true;
    public bool displayPagingToolbar { get; set; } = true;

    #region Filters

    //public Dictionary<string, string> filters { get; set; } = new Dictionary<string, string>();
    public List<QueryFilter> filters { get; set; } = new List<QueryFilter>();

    public bool hasFilter(string filterProperty)
    {
        //return this.filters.ContainsKey(filterProperty);
        return this.filters.Any(item => item.property == filterProperty);
    }

    public string filerValue(string filterProperty)
    {
        //return this.filters[filterProperty];
        return this.filters.Where(item => item.property == filterProperty).FirstOrDefault()?.value;
    }

    public class QueryFilter
    {
        public string property { get; set; }
        public string value { get; set; }
    }

    #endregion
}