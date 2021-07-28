module Main exposing (init, main, update, view)

import App.AppContainer.AppContainerView as AppContainer
import App.AppFooter.AppFooterView as AppFooter
import Browser exposing (..)
import Browser.Navigation exposing (..)
import Config exposing (..)
import Decoders exposing (..)
import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (onClick)
import Http exposing (Error(..))
import Json.Decode exposing (Decoder, field, string)
import Route exposing (..)
import Types exposing (..)
import Url exposing (Url)


getProductHomePageUrl : String
getProductHomePageUrl =
    Config.api.getProducts ++ "?Page=1&PageSize=20&includeFirstPicture=true"


init : flags -> Url.Url -> Key -> ( Model, Cmd Msg )
init flags url navKey =
    ( { currentPage = Types.HomePageNotReady
      , url = url
      , navKey = navKey
      , route = Route.parseUrl url
      }
    , Http.get { url = getProductHomePageUrl, expect = Http.expectJson InitialProductsLoad productListDecoder }
    )



-- ---------------------------
-- UPDATE
-- ---------------------------


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        InitialProductsLoad (Err error) ->
            let
                _ =
                    Debug.log "error" error
            in
            ( model, Cmd.none )

        InitialProductsLoad (Ok products) ->
            let
                homePage =
                    Types.HomePageReady products

                newModel =
                    { model | currentPage = homePage }
            in
            ( newModel, Cmd.none )

        ProductItemLoaded (Err error) ->
            let
                _ =
                    Debug.log "error" error
            in
            ( model, Cmd.none )

        ProductItemLoaded (Ok product) ->
            let
                productPage =
                    Types.ProductPageReady ( product, 1 )

                newModel =
                    { model | currentPage = productPage }
            in
            ( newModel, Cmd.none )

        UrlChange url ->
            let
                route =
                    Route.parseUrl url

                command =
                    case route of
                        HomePage ->
                            Http.get { url = getProductHomePageUrl, expect = Http.expectJson InitialProductsLoad productListDecoder }

                        ProductPage productId ->
                            Http.get { url = Config.api.getProduct ++ String.fromInt productId, expect = Http.expectJson ProductItemLoaded productItemDecoder }

                        _ ->
                            Cmd.none
            in
            ( { model | url = url, route = route }, command )

        LinkClicked urlRequest ->
            case urlRequest of
                Internal url ->
                    ( model, Browser.Navigation.pushUrl model.navKey (Url.toString url) )

                External url ->
                    ( model, Browser.Navigation.load url )

        ProductPlusOne product ->
            let
                productPageCount =
                    case model.currentPage of
                        Types.ProductPageReady ( _, currentQuantity ) ->
                            currentQuantity + 1

                        _ ->
                            0

                productPage =
                    Types.ProductPageReady ( product, productPageCount )

                newModel =
                    { model | currentPage = productPage }
            in
            ( newModel, Cmd.none )

        ProductMinusOne product ->
            let
                productPageCount =
                    case model.currentPage of
                        Types.ProductPageReady ( _, currentQuantity ) ->
                            case currentQuantity of
                                1 ->
                                    1

                                _ ->
                                    currentQuantity - 1

                        _ ->
                            0

                productPage =
                    Types.ProductPageReady ( product, productPageCount )

                newModel =
                    { model | currentPage = productPage }
            in
            ( newModel, Cmd.none )



-- ---------------------------
-- VIEW
-- ---------------------------


view : Model -> Html Msg
view model =
    div [ class "main-container" ]
        [ AppContainer.view model
        , AppFooter.view model
        , div [ class "footer-spacer" ] []
        ]



-- ---------------------------
-- MAIN
-- ---------------------------


main : Program Int Model Msg
main =
    Browser.application
        { init = init
        , view =
            \m ->
                { title = "Mes Jolis Cotillons"
                , body = [ view m ]
                }
        , update = update
        , subscriptions = \_ -> Sub.none
        , onUrlRequest = LinkClicked
        , onUrlChange = UrlChange
        }
