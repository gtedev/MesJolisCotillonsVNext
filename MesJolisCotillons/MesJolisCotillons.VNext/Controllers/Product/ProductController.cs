namespace MesJolisCotillons.VNext.Controllers.Product
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests.Product.Create;
    using MesJolisCotillons.Contracts.Requests.Product.Delete;
    using MesJolisCotillons.Contracts.Requests.Product.Get;
    using MesJolisCotillons.Contracts.Responses.Product.Create;
    using MesJolisCotillons.Contracts.Responses.Product.Delete;
    using MesJolisCotillons.Contracts.Responses.Product.Get;
    using MesJolisCotillons.Operations.Product.Create;
    using MesJolisCotillons.Operations.Product.Delete;
    using MesJolisCotillons.Operations.Product.Get;
    using MesJolisCotillons.VNext.Controllers.Service;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/products")]
    public class ProductController : ApiController
    {
        public ProductController(IControllerService controllerService)
            : base(controllerService)
        {
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns>A list of products.</returns>
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetProducts(GetProductsRequest request)
            => await this.controllerService
                .ExecuteOperationAsync<GetProductsRequest, GetProductsResponse, GetProductsOperation>(request);

        /// <summary>
        /// Create a product.
        /// </summary>
        /// <returns>A response mentionning created product.</returns>
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductRequest request)
            => await this.controllerService
                .ExecuteOperationAsync<CreateProductRequest, CreateProductResponse, CreateProductOperation>(request);

        /// <summary>
        /// Get a product.
        /// </summary>
        /// <returns>A product.</returns>
        [Produces("application/json")]
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var request = new GetProductRequest { ProductId = productId };

            return await this.controllerService
                .ExecuteOperationAsync<GetProductRequest, GetProductResponse, GetProductOperation>(request);
        }

        /// <summary>
        /// Delete a product.
        /// </summary>
        /// <returns>A product.</returns>
        [Produces("application/json")]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var request = new DeleteProductRequest { ProductId = productId };

            return await this.controllerService
                .ExecuteOperationAsync<DeleteProductRequest, DeleteProductResponse, DeleteProductOperation>(request);
        }
    }
}