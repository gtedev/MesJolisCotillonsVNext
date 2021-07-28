module ChecksProduct
 open Xunit
 open MesJolisCotillons.Contracts.ViewModels.Product
  
 let checkProductWith (expectedProduct:ProductViewModel)(actualProduct :ProductViewModel) =
    Assert.Equal(expectedProduct.Name, actualProduct.Name)
    Assert.Equal(expectedProduct.Description, actualProduct.Description)
    Assert.Equal(expectedProduct.Price, actualProduct.Price)


