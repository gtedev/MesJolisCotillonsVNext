module ArrangeProducts

open MesJolisCotillons.Contracts.Requests.Product.Create
open Products

/// <summary>
/// Returns a tuple of Product composed of its (ProductId, ProductViewModel)
/// </summary>
let ArrangeExistingProduct() = 
    let price = new System.Nullable<decimal>(decimal 20)
    let request = CreateProductRequest ( Name = "MyProduct", DisplayName = "MyDisplayName", Price = price, Description ="MyDescription")

    let createdProduct = ProductFlows.createProduct request

    (createdProduct.Product.ProductId, createdProduct.Product)

