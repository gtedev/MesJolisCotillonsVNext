using System.Linq;

namespace MesJolisCotillons.Models
{
    public interface IProductRepository
    {
        Product FindProduct(int id, Product_Status? status = null);

        IQueryable<Product> FindAllProduct();
    }
}
