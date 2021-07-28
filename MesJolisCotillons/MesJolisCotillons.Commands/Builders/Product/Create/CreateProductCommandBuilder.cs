namespace MesJolisCotillons.Commands.Builders.Product.Create
{
    using MesJolisCotillons.Commands.Product.Create;
    using MesJolisCotillons.Contracts.Requests.Product.Create;

    public class CreateProductCommandBuilder : ICreateProductCommandBuilder
    {
        public CreateProductCommand Build(CreateProductRequest request)
        {
            return new CreateProductCommand
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                DisplayName = request.DisplayName
            };
        }
    }
}
