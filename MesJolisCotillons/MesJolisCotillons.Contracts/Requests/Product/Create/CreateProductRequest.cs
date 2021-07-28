namespace MesJolisCotillons.Contracts.Requests.Product.Create
{
    using System.ComponentModel.DataAnnotations;

    public class CreateProductRequest : IRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}
