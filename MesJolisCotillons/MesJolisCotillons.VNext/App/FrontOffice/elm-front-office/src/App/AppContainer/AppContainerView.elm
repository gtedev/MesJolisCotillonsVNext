module App.AppContainer.AppContainerView exposing (view)

import App.AppHeader.AppHeaderView as AppHeader
import App.AppMain.AppMainView as AppMain
import Html exposing (..)
import Html.Attributes exposing (..)
import Http exposing (Error(..))
import Types exposing (..)


view : Model -> Html Msg
view model =
    div [ class "app-container" ]
        [ AppHeader.view model
        , AppMain.view model
        , div [ class "bandeau-shipping-infos" ]
            [ div [ class "container" ]
                [ div [ class "row bandeau-row" ]
                    [ div [ class "col-md-6 bandeau-box" ]
                        [ div []
                            [ img [ class "transport-logo", src "images/transport.png" ] []
                            , img [ class "map-logo", src "images/map-france.png" ] []
                            ]
                        , div [ class "bandeau-box-title" ] [ text "#Rapidité" ]
                        , div [] [ text "Livraison dans toute" ]
                        , div [] [ text "la France Métropolitaine" ]
                        ]
                    , div [ class "col-md-6 bandeau-box" ]
                        [ img [ class "paypal-logo", src "images/paypal.png" ] []
                        , div [ class "bandeau-box-title" ] [ text "#Sécurité" ]
                        , div [] [ text "Paiement sur Paypal" ]
                        , div [] [ text "Sans ouvrir de compte Paypal" ]
                        ]
                    ]
                ]
            ]
        ]
