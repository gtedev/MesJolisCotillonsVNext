module App.AppFooter.AppFooterView exposing (view)

import Html exposing (..)
import Html.Attributes exposing (..)
import Http exposing (Error(..))
import Types exposing (..)


view : Model -> Html Msg
view model =
    footer [ class "app-footer" ]
        [ div [ class "container" ]
            [ div [ class "row mt-5" ]
                [ div [ class "footer-item col-md-4" ]
                    [ div [ class "footer-title" ]
                        [ a [ class "animated-left-to-right-link", href "" ] [ text "LA BOUTIQUE" ]
                        ]
                    , div []
                        [ a [ class "animated-left-to-right-link", href "/a-propos" ] [ text "A propos" ] ]
                    , div []
                        [ a [ class "animated-left-to-right-link", href "/contact" ] [ text "Contact" ] ]
                    ]
                , div [ class "footer-item col-md-4" ]
                    [ div [ class "footer-title" ]
                        [ a [ class "animated-left-to-right-link", href "/besoin-d-aides" ] [ text "BESOIN D'AIDE" ] ]
                    , div []
                        [ a [ class "animated-left-to-right-link", href "/faq" ] [ text "Questions / Reponses" ] ]
                    , div []
                        [ a [ class "animated-left-to-right-link", href "/mentions-legales" ] [ text "Mentions legales" ] ]
                    ]
                , div [ class "footer-item col-md-4" ]
                    [ div [ class "footer-title" ]
                        [ a [ class "animated-left-to-right-link", href "/mon-compte" ] [ text "MON COMPTE" ] ]
                    , div []
                        [ a [ class "animated-left-to-right-link", href "/mes-commandes" ] [ text "Mes commandes" ] ]
                    , div []
                        [ a [ class "animated-left-to-right-link", href "/mes-informations" ] [ text "Mes informations" ] ]
                    ]
                ]
            , div [ class "row mt-5" ]
                [ div [ class "footer-item col mt-5" ]
                    [ div [ class "footer-title" ]
                        [ div [] [ text "SUIVEZ NOUS" ]
                        ]
                    , div []
                        [ a [ target "_blank", href "https://www.facebook.com/mesjoliscotillons/?ref=aymt_homepage_panel" ]
                            [ img [ class "footer-logo fb-logo", src "images/facebook-logo.png" ] [] ]
                        , a [ target "_blank", href "https://www.instagram.com/mesjoliscotillons/" ]
                            [ img [ class "footer-logo fb-logo", src "images/instagram-logo.png" ] [] ]
                        ]
                    ]
                ]
            ]
        ]
