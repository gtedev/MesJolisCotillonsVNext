namespace MesJolisCotillons.Commands.Builders.Product.Get
{
    using System.Collections.Generic;
    using System.Linq;
    using MesJolisCotillons.Adapters.Product.Get;
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Contracts.Requests.Product.Get;

    public class GetProductsCommandBuilder : IGetProductsCommandBuilder
    {
        public GetProductsCommand Build(IGetProductsAdapter adapter, GetProductsRequest request)
        {
            IReadOnlyCollection<int> nonExistingCategories = new List<int>();
            if (request.ProductCategories != null)
            {
                var existingCategoriesIds = adapter.ExistingCategories.Select(cc => cc.CategoryId);
                nonExistingCategories = request
                    .ProductCategories
                    .Where(c => !existingCategoriesIds.Contains(c))
                    .ToList();
            }

            return new GetProductsCommand(
                request.Page,
                request.PageSize,
                request.ProductCategories,
                nonExistingCategories,
                request.IncludeFirstPicture);
        }
    }
}
