using AutoMapper;

namespace MesJolisCotillons.DataAccess.Repositories.UnitTests.Repositories.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MesJolisCotillons.Common.UnitTests.MockMesJolisCotillonsContext;
    using MesJolisCotillons.Contracts.ViewModels.Product;
    using MesJolisCotillons.DataAccess.Repositories.AutoMapper;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;
    using Xunit;
    using E = Entities.EntityModels;

    public class ProductRepositoryTests
    {
        public abstract class ProductRepositoryTest
        {
            public ProductRepository ProductRepository { get; set; }

            public MockMesJolisCotillonsContext MockMesJolisCotillonsContext { get; set; }

            public IMapper Mapper { get; set; }

            public ProductRepositoryTest()
            {
                this.MockMesJolisCotillonsContext = new MockMesJolisCotillonsContext();

                // Arrange autoMapper
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<RepositoriesProfile>();
                });
                IMapper mapper = new Mapper(config);

                this.ProductRepository = new ProductRepository(this.MockMesJolisCotillonsContext, mapper);
            }

            protected void ArrangeDbContextWithFourProducts()
            {
                var fakeProductOne = new E.Product
                {
                    ProductId = 1,
                    Name = "PotDeFleur",
                    Description = "Ceci est un pot de fleur.",
                    Price = 20,
                    DisplayName = "Pot de fleur"
                };

                var fakeProductTwo = new E.Product
                {
                    ProductId = 2,
                    Name = "TableBois",
                    Description = "Ceci est une table en bois.",
                    Price = 50,
                    DisplayName = "Table en bois"
                };

                var fakeProductThree = new E.Product
                {
                    ProductId = 3,
                    Name = "SuperNintendo",
                    Description = "Ceci est une console.",
                    Price = 70,
                    DisplayName = "Super Nintendo"
                };

                var fakeProductFour = new E.Product
                {
                    ProductId = 4,
                    Name = "Lampe",
                    Description = "Ceci est une Lampe.",
                    Price = 50,
                    DisplayName = "La Lampe"
                };

                var listProducts = new List<E.Product>();
                listProducts.Add(fakeProductOne);
                listProducts.Add(fakeProductTwo);
                listProducts.Add(fakeProductThree);
                listProducts.Add(fakeProductFour);

                this.MockMesJolisCotillonsContext.Products.AddRange(listProducts);

                var productBlobOne = new E.ProductBlob
                {
                    ProductFk = 1,
                    BlobFk = 1
                };

                var productBlobTwo = new E.ProductBlob
                {
                    ProductFk = 2,
                    BlobFk = 2
                };
                var listProductBlobs = new List<E.ProductBlob>();
                listProductBlobs.Add(productBlobOne);
                listProductBlobs.Add(productBlobTwo);
                this.MockMesJolisCotillonsContext.ProductBlob.AddRange(listProductBlobs);

                var blobOne = new E.Blob
                {
                    BlobId = 1,
                    Stream = this.GetRandomBytesArray()
                };

                var blobTwo = new E.Blob
                {
                    BlobId = 2,
                    Stream = this.GetRandomBytesArray()
                };
                var listBlobs = new List<E.Blob>();
                listBlobs.Add(blobOne);
                listBlobs.Add(blobTwo);

                this.MockMesJolisCotillonsContext.Blob.AddRange(listBlobs);

                var categoryOne = new E.Category
                {
                    CategoryId = 1,
                    Name = "CategoryOne",
                };

                var categoryTwo = new E.Category
                {
                    CategoryId = 2,
                    Name = "CategoryOne",
                };

                var listCategories = new List<E.Category>();
                listCategories.Add(categoryOne);
                listCategories.Add(categoryTwo);

                var productCategoryOne = new E.ProductCategory
                {
                    CategoryFk = 1,
                    ProductFk = 1,
                };

                var productCategoryTwo = new E.ProductCategory
                {
                    CategoryFk = 2,
                    ProductFk = 2
                };

                var productCategoryThree = new E.ProductCategory
                {
                    CategoryFk = 2,
                    ProductFk = 3,
                };

                var listProductCategories = new List<E.ProductCategory>();
                listProductCategories.Add(productCategoryOne);
                listProductCategories.Add(productCategoryTwo);
                listProductCategories.Add(productCategoryThree);

                this.MockMesJolisCotillonsContext.ProductCategories.AddRange(listProductCategories);

                this.MockMesJolisCotillonsContext.SaveChanges();
            }

            private byte[] GetRandomBytesArray()
            {
                Random rnd = new Random();
                Byte[] randomArray = new Byte[10];
                rnd.NextBytes(randomArray);

                return randomArray;
            }
        }

        public class FindProductShould : ProductRepositoryTest
        {
            [Fact]
            public async Task ExpectedViewModelTypeAsync()
            {
                // Arrange
                this.ArrangeDbContextWithFourProducts();

                // Act
                var product = await this.ProductRepository.FindProduct(1);

                // Assert
                product.Should().BeOfType<ProductViewModel>();
                product.ProductId.Should().Be(1);
            }

            [Fact]
            public async Task ExpectedUserDatasAsync()
            {
                // Arrange
                this.ArrangeDbContextWithFourProducts();

                // Act
                var product = await this.ProductRepository.FindProduct(1);

                // Assert
                product.ProductId.Should().Be(1);
                product.Name.Should().Be("PotDeFleur");
                product.Description.Should().Be("Ceci est un pot de fleur.");
                product.Price.Should().Be(20);
                product.DisplayName.Should().Be("Pot de fleur");
            }

            [Fact]
            public async Task Returns_Null_When_User_DoesNotExist()
            {
                // Arrange
                this.ArrangeDbContextWithFourProducts();

                // Act
                var product = await this.ProductRepository.FindProduct(100);

                // Assert
                product.Should().BeNull();
            }
        }

        public class FindAllProductShould : ProductRepositoryTest
        {
            [Fact]
            public async Task ExpectedViewModelTypeAsync()
            {
                // Arrange
                this.ArrangeDbContextWithFourProducts();

                // Act
                var allProducts = await this.ProductRepository.FindAllProducts();

                // Assert
                allProducts.Should().HaveCount(4);
            }

            [Fact]
            public async Task ExpectedUserDatasAsync()
            {
                // Arrange
                this.ArrangeDbContextWithFourProducts();

                // Act
                var allProducts = await this.ProductRepository.FindAllProducts();

                // Assert
                var productOne = allProducts.ToList()[0];
                var productTwo = allProducts.ToList()[1];
                var productThree = allProducts.ToList()[2];
                var productFour = allProducts.ToList()[3];

                productOne.ProductId.Should().Be(1);
                productOne.Name.Should().Be("PotDeFleur");
                productOne.Description.Should().Be("Ceci est un pot de fleur.");
                productOne.Price.Should().Be(20);
                productOne.DisplayName.Should().Be("Pot de fleur");
                productOne.ProductBase64Images.Should().BeNull();

                productTwo.ProductId.Should().Be(2);
                productTwo.Name.Should().Be("TableBois");
                productTwo.Description.Should().Be("Ceci est une table en bois.");
                productTwo.Price.Should().Be(50);
                productTwo.DisplayName.Should().Be("Table en bois");
                productTwo.ProductBase64Images.Should().BeNull();

                productThree.ProductId.Should().Be(3);
                productThree.Name.Should().Be("SuperNintendo");
                productThree.Description.Should().Be("Ceci est une console.");
                productThree.Price.Should().Be(70);
                productThree.DisplayName.Should().Be("Super Nintendo");
                productThree.ProductBase64Images.Should().BeNull();

                productFour.ProductId.Should().Be(4);
                productFour.Name.Should().Be("Lampe");
                productFour.Description.Should().Be("Ceci est une Lampe.");
                productFour.Price.Should().Be(50);
                productFour.DisplayName.Should().Be("La Lampe");
                productFour.ProductBase64Images.Should().BeNull();
            }

            [Fact]
            public async Task ExpectedProducts_With_FirstImage_When_includeFirstPicture_IsTrue_AndWhenTheyExist()
            {
                // Arrange
                this.ArrangeDbContextWithFourProducts();

                // Act
                var allProducts = await this.ProductRepository.FindAllProducts(includeFirstPicture: true);

                // Assert
                var productOne = allProducts.ToList()[0];
                var productTwo = allProducts.ToList()[1];
                var productThree = allProducts.ToList()[2];
                var productFour = allProducts.ToList()[3];

                productOne.ProductBase64Images.Should().NotBeNull();
                productTwo.ProductBase64Images.Should().NotBeNull();
                productThree.ProductBase64Images.Should().NotBeNull();
                productFour.ProductBase64Images.Should().NotBeNull();

                productOne.ProductBase64Images.ElementAt(0).Should().NotBeEmpty();
                productTwo.ProductBase64Images.ElementAt(0).Should().NotBeEmpty();
                productThree.ProductBase64Images.ElementAt(0).Should().BeEmpty();
                productFour.ProductBase64Images.ElementAt(0).Should().BeEmpty();
            }

            [Fact]
            public async Task ExpectedProductsCount_With_One_ProductCategory()
            {
                // Arrange
                this.ArrangeDbContextWithFourProducts();
                var categoriesFilter = new List<int> { 1 };

                // Act
                var products = await this.ProductRepository.FindAllProducts(false, categoriesFilter);

                // Assert
                products.Should().HaveCount(1);
            }

            [Fact]
            public async Task ExpectedProductsCount_With_Another_ProductCategory()
            {
                // Arrange
                this.ArrangeDbContextWithFourProducts();
                var categoriesFilter = new List<int> { 2 };

                // Act
                var products = await this.ProductRepository.FindAllProducts(false, categoriesFilter);

                // Assert
                products.Should().HaveCount(2);
            }

            [Fact]
            public async Task ExpectedProductsCount_With_Many_ProductCategory()
            {
                // Arrange
                this.ArrangeDbContextWithFourProducts();
                var categoriesFilter = new List<int> { 1, 2 };

                // Act
                var products = await this.ProductRepository.FindAllProducts(false, categoriesFilter);

                // Assert
                products.Should().HaveCount(3);
            }

            public class CreateProductShould : ProductRepositoryTest
            {
                [Fact]
                public async Task BeAdded_In_Context_WithExpectedData()
                {
                    // Arrange
                    var name = "PotDeFleur";
                    var description = "Ceci est un pot de fleur.";
                    var price = 20;
                    var displayName = "Pot de fleur";

                    // Act
                    await this.ProductRepository.CreateProduct(
                        name,
                        description,
                        price,
                        displayName);

                    this.MockMesJolisCotillonsContext.SaveChanges();

                    // Assert
                    var productOne = this.MockMesJolisCotillonsContext.Products.FirstOrDefault();

                    this.MockMesJolisCotillonsContext.Products.Should().HaveCount(1);

                    productOne.ProductId.Should().Be(1);
                    productOne.Name.Should().Be(name);
                    productOne.Description.Should().Be(description);
                    productOne.Price.Should().Be(price);
                    productOne.DisplayName.Should().Be(displayName);
                }

                [Fact]
                public async Task Returns_A_Function_That_Returns_Expected_ProductViewModel_WhenExecuted()
                {
                    // Arrange
                    var name = "PotDeFleur";
                    var description = "Ceci est un pot de fleur.";
                    var price = 20;
                    var displayName = "Pot de fleur";

                    // Act
                    var productViewResolver = await this.ProductRepository.CreateProduct(
                        name,
                        description,
                        price,
                        displayName);

                    this.MockMesJolisCotillonsContext.SaveChanges();

                    // Assert
                    var productView = productViewResolver();

                    productView.ProductId.Should().BeGreaterThan(0);
                    productView.Name.Should().Be(name);
                    productView.Description.Should().Be(description);
                    productView.Price.Should().Be(price);
                    productView.DisplayName.Should().Be(displayName);
                }
            }

            public class DeleteProductShould : ProductRepositoryTest
            {
                [Fact]
                public async Task BeRemoved_From_Context()
                {
                    // Arrange
                    this.ArrangeDbContextWithFourProducts();

                    // Act
                    await this.ProductRepository.DeleteProduct(1);

                    this.MockMesJolisCotillonsContext.SaveChanges();

                    // Assert
                    var productOne = this.MockMesJolisCotillonsContext.Products
                        .Where(p => p.ProductId == 1)
                        .FirstOrDefault();

                    this.MockMesJolisCotillonsContext.Products.Should().HaveCount(3);

                    productOne.Should().BeNull();
                }

                [Fact]
                public async Task Throw_An_Exception_When_It_Cant_Find_the_Product()
                {
                    // Arrange
                    var dummyProductId = 10000;

                    // Act
                    try
                    {
                        await this.ProductRepository.DeleteProduct(dummyProductId);
                    }
                    catch (System.Exception ex)
                    {
                        // Assert
                        ex.Message.Should().Be($"DeleteProduct: Cannot find the product with ProductId '{dummyProductId}'");
                    }
                }
            }
        }
    }
}
