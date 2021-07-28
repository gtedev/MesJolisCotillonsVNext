using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Direct.Mvc;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ext.Direct.Mvc
{
    public class DirectStoreQuery
    {
        public class Filter
        {
            public String property { get; set; }
            public String value { get; set; }
        }

        public class SortFilter
        {
            public String property { get; set; }
            public String direction { get; set; }
        }

        public List<Filter> filter { get; set; }

        public List<SortFilter> sort { get; set; }

        //public string name { get; set; }
        public int? start { get; set; }
        public int? limit { get; set; }
        public int? page { get; set; }
        /// <summary>
        /// This property will only be passed by a store on a combobox
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// This property will only be passed by a treestore, from a tree
        /// </summary>
        //public int node { get; set; }

        public string node { get; set; }

        /// <summary>
        /// Get the value of the passed property from the filter
        /// </summary>
        /// <param name="property">the name of the property we are looking for</param>
        /// <returns>the value paired to the filter property</returns>
        public string filterValue(string property)
        {
            if (filter == null) return null;
            var tmp = filter
                    .Where(f => !String.IsNullOrWhiteSpace(f.property))
                    .Where(f => f.property.ToUpperInvariant() == property.ToUpperInvariant()).FirstOrDefault();
            //return tmp == null ? String.Empty : tmp.value;
            return tmp == null ? String.Empty : tmp.value;
        }
        public bool filterHasProperty(string property)
        {
            if (filter == null) return false;
            return filter
                .Where(f => !String.IsNullOrWhiteSpace(f.property))
                .Any(f => f.property.ToUpperInvariant() == property.ToUpperInvariant());
        }

    }

    public class DirectModelQuery
    {
        public int id { get; set; }
    }

    public class DirectResponseResult
    {
        public enum MsgType : int
        {
            warning = 1,
            information = 2,
            success = 4
        }

        private object _data;

        public bool success { get; set; }
        public string msg { get; set; }
        public object data
        {
            get
            {
                return _data == null ? new { } : _data;
            }
            set
            {
                _data = value;
            }
        }
        public int total { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public MsgType msgType { get; set; }

    }
}