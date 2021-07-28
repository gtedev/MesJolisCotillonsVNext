namespace MesJolisCotillons.Commands.Product.Get
{
    using System.Collections.Generic;
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public class GetProductsCommand : ICommand,
        IExistingCategoriesCommand,
        IProductsPagedCommand
    {
        public GetProductsCommand(
            int page,
            int pageSize,
            IReadOnlyCollection<int> productCategories,
            IReadOnlyCollection<int> nonExistingProductCategories,
            bool includeFirstPicture)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.ProductCategories = productCategories;
            this.NonExistingCategories = nonExistingProductCategories;
            this.IncludeFirstPicture = includeFirstPicture;
        }

        public int Page { get; }

        public int PageSize { get; }

        public IReadOnlyCollection<int> ProductCategories { get; }

        public IReadOnlyCollection<int> NonExistingCategories { get; }

        public IReadOnlyCollection<ProductViewModel> ProductsResult { get; set; }

        public bool IncludeFirstPicture { get; }
    }
}
