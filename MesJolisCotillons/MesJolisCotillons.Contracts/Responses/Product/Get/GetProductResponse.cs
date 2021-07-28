using MesJolisCotillons.Contracts.ViewModels.Product;

namespace MesJolisCotillons.Contracts.Responses.Product.Get
{
    public class GetProductResponse : ResponseBase
    {
        public GetProductResponse(bool success)
            : base(success)
        {

        }

        public ProductViewModel Product { get; set; }
    }
}
