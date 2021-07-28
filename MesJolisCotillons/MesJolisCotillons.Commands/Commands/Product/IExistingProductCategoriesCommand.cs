namespace MesJolisCotillons.Commands.Product
{
    using System.Collections.Generic;

    public interface IExistingCategoriesCommand : ICommand
    {
        IReadOnlyCollection<int> NonExistingCategories { get; }
    }
}
