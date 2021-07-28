module Types exposing (..)

import Browser
import Browser.Navigation exposing (..)
import Html exposing (..)
import Html.Attributes exposing (..)
import Http exposing (Error(..))
import Route exposing (..)
import Url exposing (Url)


type alias Product =
    { productId : Int
    , name : String
    , displayName : String
    , description : String
    , price : Float
    , productBase64Images : List String
    }


type Msg
    = UrlChange Url
    | LinkClicked Browser.UrlRequest
    | InitialProductsLoad (Result Http.Error (List Product))
    | ProductItemLoaded (Result Http.Error Product)
    | ProductPlusOne Product
    | ProductMinusOne Product


type AppMainPageModel
    = HomePageNotReady
    | HomePageReady (List Product)
    | ProductPageNotReady
    | ProductPageReady ( Product, Int )


type alias Model =
    { currentPage : AppMainPageModel
    , navKey : Key
    , url : Url
    , route : Route
    }
