using MesJolisCotillons.Contracts.ViewModels.Product;
using System;

namespace MesJolisCotillons.Commands.Product.Create
{
    public class CreateProductCommand : ICommand
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public string DisplayName { get; set; }

        /// <summary>
        /// Function that will be set by executor to get ProductViewModel for the created product, and will be used in ResponseBuilder.
        /// This function needs to be executed only when SaveChanges occured.
        /// </summary>
        public Func<ProductViewModel> ProductViewResolver { get; set; }
    }
}
