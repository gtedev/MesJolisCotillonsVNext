module App.AppMain.Pages.MyCommands.MyCommandsView exposing (view)

import Html exposing (..)
import Html.Attributes exposing (..)
import Types exposing (..)


view : Model -> List (Html Msg)
view model =
    [ div [] [ text "MyCommands" ] ]
