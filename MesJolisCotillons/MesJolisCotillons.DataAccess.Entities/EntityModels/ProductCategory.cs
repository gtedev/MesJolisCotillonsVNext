namespace MesJolisCotillons.DataAccess.Entities.EntityModels
{
    public class ProductCategory
    {
        public int ProductFk { get; set; }

        public int CategoryFk { get; set; }

        public Category CategoryFkNavigation { get; set; }

        public Product ProductFkNavigation { get; set; }
    }
}
