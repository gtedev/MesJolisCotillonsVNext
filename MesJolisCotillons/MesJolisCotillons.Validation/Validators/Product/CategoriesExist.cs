namespace MesJolisCotillons.Validation.Validators.Product
{
    using System.Linq;
    using MesJolisCotillons.Commands.Product;
    using MesJolisCotillons.Contracts;

    public class CategoriesExist : IValidator<IExistingCategoriesCommand>
    {
        public MessageCode MessageFailureCode => MessageCode.CategoriesDoNotExist;

        public bool Validate(IExistingCategoriesCommand command)
            => command.NonExistingCategories.Count() == 0;
    }
}
