namespace  Products 

open IntegrationTestsMiddleware
open MesJolisCotillons.Contracts.Requests.Product.Create
open MesJolisCotillons.Contracts.Responses.Product.Create
open MesJolisCotillons.Contracts.Responses.Product.Delete

module ProductFlows =

 open Types
 open Checks
 open FSharp.Data
 open MesJolisCotillons.Contracts.Responses.Product.Get

 /// <summary>
 /// Get all products via MesJolisCotillons API.
 /// </summary>
 let getProducts queryParameters =

    createMesJolisCotillonsRequest MjcApi.Api.Products.GetProducts   
    |> withHttpMethod HttpMethod.GET
    |> withQueryParameters queryParameters
    |> send<GetProductsResponse>
    |> checkHttpStatus HttpStatus.Success
    |> toResponse

/// <summary>
/// Create a product via MesJolisCotillons API.
/// </summary>
 let createProduct (request: CreateProductRequest) =

    createMesJolisCotillonsRequest MjcApi.Api.Products.CreateProduct   
    |> withHttpPostMethod
    |> withBodyRequest request
    |> send<CreateProductResponse>
    |> checkHttpStatus HttpStatus.Success
    |> toResponse

/// <summary>
/// Get a product via MesJolisCotillons API.
/// </summary>
 let getProduct (productId:int) =

    createMesJolisCotillonsRequest (MjcApi.Api.Products.GetProduct + productId.ToString())   
    |> withHttpMethod HttpMethod.GET
    |> send<GetProductResponse>
    |> checkHttpStatus HttpStatus.Success
    |> toResponse

/// <summary>
/// Delete a product via MesJolisCotillons API.
/// </summary>
 let deleteProduct (productId:int) =
    
    createMesJolisCotillonsRequest (MjcApi.Api.Products.DeleteProduct + productId.ToString())   
    |> withHttpMethod HttpMethod.DELETE
    |> send<DeleteProductResponse>
    |> checkHttpStatus HttpStatus.Success
    |> toResponse