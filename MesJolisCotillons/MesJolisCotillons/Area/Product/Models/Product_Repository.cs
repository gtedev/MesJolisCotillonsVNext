using System.Linq;


namespace MesJolisCotillons.Models
{
    public class Product_Repository : Repository, IProductRepository
    {
        public IQueryable<Product> FindAllProduct()
        {
            return db.Products;
        }

        public IQueryable<Category> FindAllCategory()
        {
            return db.Categories;
        }

        public void InsertBlob(Blob blob)
        {
            db.Blobs.Add(blob);
        }

        public Product FindProduct(int id, Product_Status? status = null)
        {
            Product product = null;
            if (status != null)
            {
                product = db.Products.Where(item => item.Product_ID == id && item.Status == (Product_Status)status)
                         .FirstOrDefault();
            }
            else
            {
                product = db.Products.Where(item => item.Product_ID == id)
                         .FirstOrDefault();
            }

            return product;
        }

        internal void deleteBlob(Blob blob)
        {
            db.Blobs.Remove(blob);
        }

        public Category FindCategory(int Category_FK)
        {
            return FindAllCategory().Where(item => item.Category_ID == Category_FK)
                                    .FirstOrDefault();
        }

        public void addProduct(Product product)
        {
            db.Products.Add(product);
        }

        public void addCategory(Category category)
        {
            db.Categories.Add(category);
        }

        public void deleteProduct(Product product)
        {
            foreach (var blob in product.Blob_Set.ToList())
            {
                product.Blob_Set.Remove(blob);
                this.db.Blobs.Remove(blob);
            }
            foreach (var keyWord in product.KeyWord_Set.ToList())
            {
                product.KeyWord_Set.Remove(keyWord);
            }

            foreach (Category category in product.Category_Set.ToList<Category>())
            {
                product.Category_Set.Remove(category);
            }

            this.db.Products.Remove(product);
        }

        public void deleteCategory(Category category)
        {
            this.db.Categories.Remove(category);
        }

        public IQueryable<KeyWord> FindAllKeyWord()
        {
            return this.db.KeyWords;
        }

        public KeyWord FindKeyWord(int KeyWord_ID)
        {
            return this.db.KeyWords.Where(item => item.KeyWord_ID == KeyWord_ID)
                                   .FirstOrDefault();
        }

        public void addKeyWord(KeyWord keyWord)
        {
            this.db.KeyWords.Add(keyWord);
        }

        public void deleteKeyWord(KeyWord keyWord)
        {
            this.db.KeyWords.Remove(keyWord);
        }

        public ProductAvailabilityResponse checkProductAvailability(ProductAvailability form)
        {

            var product = FindProduct(form.Product_ID);
            var result = new ProductAvailabilityResponse
            {
                Product_ID = form.Product_ID,
                isAvailable = product != null && form.Quantity <= product.StockQuantity
            };

            return result;
        }
    }

    public class ProductAvailability
    {
        public int Product_ID { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductAvailabilityResponse
    {
        public int Product_ID { get; set; }
        public bool isAvailable { get; set; }
    }
}