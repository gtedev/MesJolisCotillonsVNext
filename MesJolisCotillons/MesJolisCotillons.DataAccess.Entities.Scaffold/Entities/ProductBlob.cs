using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class ProductBlob
    {
        public int ProductFk { get; set; }
        public int BlobFk { get; set; }

        public Blob BlobFkNavigation { get; set; }
        public Product ProductFkNavigation { get; set; }
    }
}
