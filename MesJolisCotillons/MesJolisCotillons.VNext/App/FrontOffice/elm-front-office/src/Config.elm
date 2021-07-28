module Config exposing (..)


baseUrl : String
baseUrl =
    "https://localhost:44355/"


type alias ApiConfig =
    { getProducts : String
    , getProduct : String
    }


api : ApiConfig
api =
    { getProducts = baseUrl ++ "/api/products"
    , getProduct = baseUrl ++ "/api/products/"
    }
