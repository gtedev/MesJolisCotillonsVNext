using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class Etiquette
    {
        public Etiquette()
        {
            Category = new HashSet<Category>();
        }

        public int EtiquetteId { get; set; }
        public int BlobFk { get; set; }
        public string EtiquetteName { get; set; }

        public Blob BlobFkNavigation { get; set; }
        public ICollection<Category> Category { get; set; }
    }
}
