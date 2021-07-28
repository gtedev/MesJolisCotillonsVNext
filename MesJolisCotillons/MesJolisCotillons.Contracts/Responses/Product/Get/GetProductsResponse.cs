namespace MesJolisCotillons.Contracts.Responses.Product.Get
{
    using System.Collections.Generic;
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public class GetProductsResponse : ResponseBase
    {
        public GetProductsResponse(bool success)
            : base(success)
        {
        }

        public IReadOnlyCollection<ProductViewModel> Products { get; set; }
    }
}
