module App.AppHeader.AppHeaderView exposing (view)

import Html exposing (..)
import Html.Attributes exposing (..)
import Http exposing (Error(..))
import Types exposing (..)


view : Model -> Html Msg
view model =
    header [ class "app-header container" ]
        [ div [ class "logo" ]
            [ img [ class "img-logo", src "images/mjc-logo-1.png" ] []
            , strong [ class "d-none d-md-flex" ] [ text "MES JOLIS COTILLONS" ]
            ]
        , div [ class "menu d-none d-md-flex" ]
            [ div [ class "menu-item" ] [ a [ class "menu-item-link animated-left-to-right-link", href "/" ] [ text "Accueil" ] ]
            , div [ class "menu-item" ] [ a [ class "menu-item-link animated-left-to-right-link", href "" ] [ text "La Brocante" ] ]
            , div [ class "menu-item" ] [ a [ class "menu-item-link animated-left-to-right-link", href "" ] [ text "Produits" ] ]
            , div [ class "menu-item" ] [ a [ class "menu-item-link animated-left-to-right-link", href "/mon-compte" ] [ text "Mon Compte" ] ]
            , img [ class "menu-item cart-logo", src "images/tools-and-utensils.png" ] []
            , img [ class "menu-item cart-logo", src "images/sell.png" ] []
            ]
        ]
