namespace MesJolisCotillons.Validation.Validators.Product
{
    using System.Linq;
    using MesJolisCotillons.Commands.Product;
    using MesJolisCotillons.Contracts;

    public class PageParametersGreaterThanOne : IValidator<IProductsPagedCommand>
    {
        public MessageCode MessageFailureCode => MessageCode.PageParametersNotGreaterThanZero;

        public bool Validate(IProductsPagedCommand command)
            => command.Page > 0 && command.PageSize > 0;
    }
}
