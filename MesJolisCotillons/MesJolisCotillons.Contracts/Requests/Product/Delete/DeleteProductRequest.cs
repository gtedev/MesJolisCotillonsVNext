namespace MesJolisCotillons.Contracts.Requests.Product.Delete
{
    public class DeleteProductRequest : IRequest
    {
        public int ProductId { get; set; }
    }
}
