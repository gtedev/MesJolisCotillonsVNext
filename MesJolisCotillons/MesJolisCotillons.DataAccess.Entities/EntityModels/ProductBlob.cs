namespace MesJolisCotillons.DataAccess.Entities.EntityModels
{
    public class ProductBlob
    {
        public int ProductFk { get; set; }

        public int BlobFk { get; set; }

        public Blob BlobFkNavigation { get; set; }

        public Product ProductFkNavigation { get; set; }
    }
}
