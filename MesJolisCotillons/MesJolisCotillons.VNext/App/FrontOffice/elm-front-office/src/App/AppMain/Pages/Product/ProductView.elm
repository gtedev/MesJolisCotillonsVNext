module App.AppMain.Pages.Product.ProductView exposing (view)

import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (onClick)
import Types exposing (..)
import Utils.Product.ProductUtils as ProductUtils exposing (..)


viewProductDetailsTabPanel : Product -> Html Msg
viewProductDetailsTabPanel product =
    let
        littleImageItems =
            product.productBase64Images
                |> List.map (\productImg -> img [ class "product-image-placeholder", src (toSrcImage productImg) ] [])
    in
    div [ class "product-item-tabs col" ]
        [ div [ class "tabs" ]
            [ div [ class "tab selected" ] [ text "Détails produit" ], div [ class "tab" ] [ text "Livraison" ] ]
        , div [ class "tab-content" ] []
        ]


viewProductInfosPanel : Product -> Int -> Html Msg
viewProductInfosPanel product quantity =
    div [ class "col-md-6 product-info-panel" ]
        [ div [ class "product-item-title" ] [ text product.displayName ]
        , div [ class "product-item-description" ] [ text product.description ]
        , div [ class "product-item-price" ] [ text (String.fromFloat product.price ++ " €") ]
        , div [ class "add-to-cart-quantity" ]
            [ div [ class "add-to-cart-quantity-container" ]
                [ div [ class "quantity" ] [ text (String.fromInt quantity) ]
                , div [ class "add-minus-buttons" ]
                    [ div [ class "plus", onClick (ProductPlusOne product) ] [ i [ class "arrow up" ] [] ]
                    , div [ class "minus", onClick (ProductMinusOne product) ] [ i [ class "arrow down" ] [] ]
                    ]
                ]
            ]
        , div [ class "add-to-cart-button" ] [ text "Ajouter au panier" ]
        , div [ class "share-product-item" ]
            [ div [] [ text "Partager ce produit:" ]
            , div [] [ text "Facebook Instagram" ]
            ]
        ]


viewProductImageSlider : Product -> Html Msg
viewProductImageSlider product =
    let
        littleImageItems =
            product.productBase64Images
                |> List.map (\productImg -> img [ class "product-image-placeholder", src (toSrcImage productImg) ] [])
                |> List.take 4

        --temporary taking 4, in order to display easily under big image without mess
    in
    div [ class "product-image-slider" ]
        littleImageItems


viewProductImagePanel : Product -> Html Msg
viewProductImagePanel product =
    let
        firstProductImage =
            ProductUtils.getFirsProductImage product
    in
    div [ class "col-md-6 product-image-slides" ]
        [ img [ class "product-image", src firstProductImage ] []
        , viewProductImageSlider product
        ]


view : Int -> Model -> List (Html Msg)
view productId model =
    let
        divProductViews =
            case model.currentPage of
                Types.ProductPageReady ( product, quantity ) ->
                    { productInfoPanelView = viewProductInfosPanel product quantity
                    , productImagesPanelView = viewProductImagePanel product
                    , productDetailTabsPanelView = viewProductDetailsTabPanel product
                    }

                Types.ProductPageNotReady ->
                    { productInfoPanelView = div [] []
                    , productImagesPanelView = div [] []
                    , productDetailTabsPanelView = div [] []
                    }

                _ ->
                    { productInfoPanelView = div [] []
                    , productImagesPanelView = div [] []
                    , productDetailTabsPanelView = div [] []
                    }
    in
    [ div [ class "product-item" ]
        [ div [ class "row" ]
            [ divProductViews.productImagesPanelView
            , divProductViews.productInfoPanelView
            ]
        , div [ class "row" ]
            [ divProductViews.productDetailTabsPanelView
            ]
        , div [ class "row" ]
            [ div [ class "related-products col" ]
                []
            ]
        ]
    ]
