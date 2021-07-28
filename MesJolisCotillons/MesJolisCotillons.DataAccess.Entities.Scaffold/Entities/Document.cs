using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class Document
    {
        public int DocumentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
        public int BlobFk { get; set; }

        public Blob BlobFkNavigation { get; set; }
        public CommandDocument CommandDocument { get; set; }
    }
}
