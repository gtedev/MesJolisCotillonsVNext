module Utils.Product.ProductUtils exposing (..)

import Types exposing (..)


toSrcImage : String -> String
toSrcImage img64Content =
    "data:image/jpg;base64," ++ img64Content


getFirsProductImage : Product -> String
getFirsProductImage product =
    let
        maybeFirstImage =
            List.head product.productBase64Images

        firstProductImage =
            case maybeFirstImage of
                Just image ->
                    toSrcImage image

                Nothing ->
                    ""
    in
    firstProductImage
