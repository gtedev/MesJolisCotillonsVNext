namespace  Products 


module ProductsTests =

 open Checks
 open Xunit
 open ChecksProduct

 [<Fact>]
 let ``GetProducts returns False when Pages parameters equal 0`` () =

    // Arrange request with 0 values for Pages parameters
    let queryParameters = [("page" , "0"); ("pageSize" , "0")] 

    // Then  success is False because parameters needs to be greater than 0.
    ProductFlows.getProducts queryParameters
    |> ``Where Success should be`` false
    |> ``Where messages count should be`` 1
    |> ``And message should be`` (sprintf "Les paramètres de Page doivent être supérieurs à 0.")

 [<Fact>]
 let ``GetProducts returns False when a Category does not exist`` () =

    // Arrange request with dummy productCategories
    let queryParameters = [("page" , "1"); ("pageSize" , "20"); ("productCategories","100000")] 

    // Then  success is False because productCategories does not exist.
    ProductFlows.getProducts queryParameters
    |> ``Where Success should be`` false
    |> ``Where messages count should be`` 1
    |> ``And message should be`` (sprintf "Une ou des catégories soumises n'existent pas.")

 [<Fact>]
 let ``GetProduct`` () =

    // Given an existing product
    let (existingProductId, existingProduct) = ArrangeProducts.ArrangeExistingProduct()

    // Then Get product by its Id, and check it
    ProductFlows.getProduct existingProductId
    |> ``Where Success should be`` true 
    |> function getProductResponse -> getProductResponse.Product
    |> checkProductWith existingProduct
  
    // Then delete product
    ProductFlows.deleteProduct existingProduct.ProductId

 [<Fact>]
 let ``DeleteProduct returns Success = True When it exists`` () =

    // Given an existing product
    let (existingProductId, _) = ArrangeProducts.ArrangeExistingProduct()

    // Then Delete product by its Id, and check its message.
    ProductFlows.deleteProduct existingProductId
    |> ``Where Success should be`` true
    |> ``Where messages count should be`` 1
    |> ``And message should be`` (sprintf "Le produit '%i' a été supprimé avec succès." existingProductId)
    |> ignore

    // Then GetProduct returns nothing.
    ProductFlows.getProduct existingProductId
    |> ``Where Success should be`` false 
    |> ``Where messages count should be`` 1
    |> ``And message should be`` (sprintf "Le produit avec l'identifiant `%i` n'existe pas." existingProductId)

 [<Fact>]
 let ``DeleteProduct returns Success = False When it does not exist`` () =

    // Given a dummy product Id
    let nonExistingProductId = 20000

    // Then Delete product by its Id, and check its message.
    ProductFlows.deleteProduct nonExistingProductId
    |> ``Where Success should be`` false
    |> ``Where messages count should be`` 1
    |> ``And message should be`` (sprintf "Le produit avec l'identifiant `%i` n'existe pas." nonExistingProductId)
  
               
 


