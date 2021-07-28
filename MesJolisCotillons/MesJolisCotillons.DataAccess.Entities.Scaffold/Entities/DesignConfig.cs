using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class DesignConfig
    {
        public int DesignConfigId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string XmlData { get; set; }
        public DateTime? CreationDatetime { get; set; }
        public DateTime? LastUpdateDatetime { get; set; }
    }
}
