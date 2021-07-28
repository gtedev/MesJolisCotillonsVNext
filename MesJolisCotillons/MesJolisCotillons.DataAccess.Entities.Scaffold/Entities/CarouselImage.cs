using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class CarouselImage
    {
        public int CarouselImageId { get; set; }
        public int BlobFk { get; set; }
        public string CarouselImageName { get; set; }

        public Blob BlobFkNavigation { get; set; }
    }
}
