module App.AppMain.Pages.Home.HomeView exposing (view)

import Html exposing (..)
import Html.Attributes exposing (..)
import Types exposing (..)
import Utils.Product.ProductUtils as ProductUtils exposing (..)


dataProductIdAttribute : Product -> Attribute msg
dataProductIdAttribute product =
    attribute "data-product-id" (String.fromInt product.productId)


getProductLink : Product -> String
getProductLink product =
    "/product/" ++ String.fromInt product.productId


mapProductToDiv : Product -> Html Msg
mapProductToDiv product =
    let
        firstProductImage =
            ProductUtils.getFirsProductImage product
    in
    div [ class "col product-grid-item", dataProductIdAttribute product ]
        [ a [ href (getProductLink product) ]
            [ img [ class "product-image", src firstProductImage ] [] ]

        -- , div [ class "product-info-overlay" ]
        --     [ div [ class "product-display-name" ] [ text product.displayName ]
        --     ]
        ]


view : Model -> List (Html Msg)
view model =
    let
        divProducts =
            case model.currentPage of
                Types.HomePageReady products ->
                    div [ class "col product-grid" ] (List.map mapProductToDiv products)

                Types.HomePageNotReady ->
                    div [ class "col product-grid" ] []

                _ ->
                    div [ class "col product-grid" ] []
    in
    [ div [ class "home-carousel d-none d-md-flex" ] []
    , divProducts
    ]
