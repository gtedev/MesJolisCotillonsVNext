module Decoders exposing (..)

import Http exposing (Error(..))
import Json.Decode exposing (..)
import Types exposing (..)


productDecoder : Decoder Product
productDecoder =
    map6 Product
        (field "productId" int)
        (field "name" string)
        (field "displayName" string)
        (field "description" (map (Maybe.withDefault "") (nullable string)))
        (field "price" float)
        (field "productBase64Images" (map (Maybe.withDefault []) (nullable (list string))))


productListDecoder : Decoder (List Product)
productListDecoder =
    field "products" (list productDecoder)


productItemDecoder : Decoder Product
productItemDecoder =
    field "product" productDecoder
