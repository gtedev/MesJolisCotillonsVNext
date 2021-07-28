namespace MesJolisCotillons.DataAccess.Repositories.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using global::AutoMapper;
    using MesJolisCotillons.Contracts.ViewModels.Product;
    using MesJolisCotillons.DataAccess.Entities.Context;
    using Microsoft.EntityFrameworkCore;
    using E = Entities.EntityModels;

    public class ProductRepository : RepositoryBase, IProductRepository
    {
        public ProductRepository(IMesJolisCotillonsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<IReadOnlyCollection<ProductViewModel>> FindAllProducts(
            bool includeFirstPicture = false,
            IReadOnlyCollection<int> categoriesFilter = null)
        {
            if (includeFirstPicture)
            {
                return await this.GetProductsWithFirstImage(categoriesFilter);
            }

            return await this.GetProducts(categoriesFilter)
                .Select(item => this.mapper.Map<ProductViewModel>(item))
                .ToListAsync();
        }

        public async Task<ProductViewModel> FindProduct(int productId, bool includePictures = false)
        {
            var product = await this.context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return null;
            }

            if (includePictures)
            {
                return await this.GetProductWithImages(product);
            }

            return this.mapper.Map<ProductViewModel>(product);
        }

        public async Task<Func<ProductViewModel>> CreateProduct(string name, string description, decimal? price, string displayName)
        {
            var product = new E.Product
            {
                Name = name,
                Description = description,
                Price = price,
                DisplayName = displayName
            };

            await this.context.Products.AddAsync(product);

            Func<ProductViewModel> productViewResolver = () =>
            {
                return this.mapper.Map<ProductViewModel>(product);
            };

            return productViewResolver;
        }

        public async Task DeleteProduct(int productId)
        {
            var product = await this.context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                throw new Exception($"DeleteProduct: Cannot find the product with ProductId '{productId}'");
            }

            // remove related entities, there is no cascade delete. 
            // EF is actually not configured like that.
            var productCategories = this.context.ProductCategories.Where(pc => pc.ProductFk == product.ProductId);
            this.context.ProductCategories.RemoveRange(productCategories);

            var productBlobs = this.context.ProductBlob.Where(pc => pc.ProductFk == product.ProductId);
            this.context.ProductBlob.RemoveRange(productBlobs);

            this.context.Products.Remove(product);
        }

        public async Task<IReadOnlyCollection<CategoryViewModel>> FindAllCategories()
        {
            var categories = this.context.Categories;

            return await this.context
                .Categories
                .Select(c => this.mapper.Map<CategoryViewModel>(c))
                .ToListAsync();
        }

        private async Task<IReadOnlyCollection<ProductViewModel>> GetProductsWithFirstImage(IReadOnlyCollection<int> categoriesFilter)
        {
            var products = await this.GetProducts(categoriesFilter)
                    .ToListAsync();

            if (products.Count() == 0)
            {
                return new List<ProductViewModel>();
            }

            var productIds = products.Select(p => p.ProductId);

            var productBlobs = await this.context.ProductBlob
                .Where(pb => productIds.Contains(pb.ProductFk))
                .GroupBy(pb => pb.ProductFk)
                .Select(g => g.FirstOrDefault())
                .ToListAsync();

            var blobIds = productBlobs.Select(pb => pb.BlobFk);
            var blobs = await this.context
                .Blob
                .Where(b => blobIds.Contains(b.BlobId))
                .ToListAsync();

            var producBlobSteam = productBlobs
                .Select(pb => new
                {
                    ProductBlob = pb,
                    Steam = blobs.FirstOrDefault(b => b.BlobId == pb.BlobFk).Stream
                }).ToDictionary(item => item.ProductBlob.ProductFk, item => item.Steam);

            var result = products.Select(p => new
            {
                Product = p,
                BlobStream = producBlobSteam.ContainsKey(p.ProductId)
                ? producBlobSteam[p.ProductId]
                : Array.Empty<byte>(),
            })
            .Select(item => this.MapProductWithPicture(item.Product, new List<byte[]> { item.BlobStream }))
            .ToList();

            return result;
        }

        private async Task<ProductViewModel> GetProductWithImages(E.Product product)
        {
            // Break down the request by querying each table separately, to minimize amount of request done to database.
            // It is not using navigation property to go from Product -> take 1st ProductBlob -> Blob
            // Avoiding problem of n+1 request.
            var productBlobs = await this.context.ProductBlob
                .Where(pb => pb.ProductFk == product.ProductId)
                .ToListAsync();

            var blobIds = productBlobs.Select(pb => pb.BlobFk);
            var blobSteams = await this.context
                .Blob
                .Where(b => blobIds.Contains(b.BlobId))
                .Select(b => b.Stream)
                .ToListAsync();

            var result = this.MapProductWithPicture(product, blobSteams);

            return result;
        }

        private ProductViewModel MapProductWithPicture(E.Product product, IEnumerable<byte[]> blobStreams)
        {
            var imageBase64s = blobStreams
                 .Select(str => Convert
                 .ToBase64String(str))
                 .ToImmutableArray();

            var productView = new ProductViewModel(
                product.ProductId,
                product.Name,
                product.Description,
                product.Price,
                product.DisplayName,
                imageBase64s);

            return productView;
        }

        /// <summary>
        /// Get products with filters like categories.
        /// </summary>
        /// <param name="categoriesFilter">Categories to filter</param>
        /// <returns>A Queryable products.</returns>
        private IQueryable<E.Product> GetProducts(IReadOnlyCollection<int> categoriesFilter)
        {
            Expression<Func<E.Product, bool>> whereCategoriesFilter =
                (product) =>
                    product.ProductCategories
                    .Select(pc => pc.CategoryFk)
                    .Intersect(categoriesFilter).Count() > 0;

            if (categoriesFilter?.Count() > 0 == true)
            {
                return this.context
                    .Products
                    .Include(p => p.ProductCategories)
                    .Where(whereCategoriesFilter);
            }
            else
            {
                return this.context.Products;
            }
        }
    }
}
