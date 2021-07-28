namespace MesJolisCotillons.DataAccess.Entities.EntityModels
{
    using System;
    using System.Collections.Generic;

    public class Blob
    {
        public Blob()
        {
            ////CarouselImage = new HashSet<CarouselImage>();
            ////Document = new HashSet<Document>();
            ////Etiquette = new HashSet<Etiquette>();
            ////Product = new HashSet<Product>();
            ProductBlob = new HashSet<ProductBlob>();
        }

        public int BlobId { get; set; }

        public byte[] Stream { get; set; }

        public string MimeType { get; set; }

        public string FileName { get; set; }

        public DateTime? CreationDateTime { get; set; }

        ////public ICollection<CarouselImage> CarouselImage { get; set; }
        ////public ICollection<Document> Document { get; set; }
        ////public ICollection<Etiquette> Etiquette { get; set; }
        ////public ICollection<Product> Product { get; set; }

        public ICollection<ProductBlob> ProductBlob { get; set; }
    }
}
