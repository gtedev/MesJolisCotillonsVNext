namespace MesJolisCotillons.DataAccess.Repositories.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public interface IProductRepository : IRepositoryBase
    {
        Task<IReadOnlyCollection<ProductViewModel>> FindAllProducts(
            bool includeFirstPicture = false,
            IReadOnlyCollection<int> categories = null);

        Task<ProductViewModel> FindProduct(int productId, bool includePictures = false);

        Task<IReadOnlyCollection<CategoryViewModel>> FindAllCategories();

        /// <summary>
        /// Create Product.
        /// <para> The method returns a function that can be executed after SaveChanges to retrieve ProductViewModel for the created product.</para>
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="price">Price.</param>
        /// <param name="displayName">DisplayName.</param>
        /// <returns>Returns a function that can be executed after SaveChanges to retrieve ProductId.</returns>
        Task<Func<ProductViewModel>> CreateProduct(string name, string description, decimal? price, string displayName);

        Task DeleteProduct(int productId);
    }
}
