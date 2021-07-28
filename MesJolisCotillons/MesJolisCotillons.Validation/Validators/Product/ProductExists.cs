namespace MesJolisCotillons.Validation.Validators.Product
{
    using MesJolisCotillons.Commands.Product;
    using MesJolisCotillons.Contracts;

    public class ProductExists : IValidator<IExistingProductCommand>
    {
        public MessageCode MessageFailureCode => MessageCode.ProductDoesNotExist;

        public bool Validate(IExistingProductCommand command)
            => command.ExistingProduct != null;
    }
}
