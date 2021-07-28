namespace MesJolisCotillons.Contracts.Requests.Product.Get
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class GetProductsRequest : IRequest
    {
        [Required]
        public int Page { get; set; }

        [Required]
        public int PageSize { get; set; }

        public IReadOnlyCollection<int> ProductCategories { get; set; }

        public bool IncludeFirstPicture { get; set; }
    }
}
