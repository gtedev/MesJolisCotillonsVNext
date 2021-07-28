namespace MesJolisCotillons.Contracts.Responses.Product.Create
{
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public class CreateProductResponse : ResponseBase
    {
        public CreateProductResponse(bool success)
            : base(success)
        {
        }

        public ProductViewModel Product { get; set; }
    }
}
