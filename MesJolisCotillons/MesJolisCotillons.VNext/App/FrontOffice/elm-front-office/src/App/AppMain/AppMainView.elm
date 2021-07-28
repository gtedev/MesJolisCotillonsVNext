module App.AppMain.AppMainView exposing (view)

import App.AppMain.Pages.About.AboutView as About
import App.AppMain.Pages.Contact.ContactView as Contact
import App.AppMain.Pages.Faq.FaqView as Faq
import App.AppMain.Pages.Home.HomeView as Home
import App.AppMain.Pages.Legals.LegalsView as Legals
import App.AppMain.Pages.MyAccount.MyAccountView as MyAccount
import App.AppMain.Pages.MyCommands.MyCommandsView as MyCommands
import App.AppMain.Pages.MyInformations.MyInformationsView as MyInformations
import App.AppMain.Pages.NeedHelp.NeedHelpView as NeedHelp
import App.AppMain.Pages.Product.ProductView as Product
import Html exposing (..)
import Html.Attributes exposing (..)
import Http exposing (Error(..))
import Route exposing (..)
import Types exposing (..)


default : Model -> List (Html Msg)
default model =
    [ div [] [ text "Default" ] ]


view : Model -> Html Msg
view model =
    let
        routedView =
            case model.route of
                HomePage ->
                    Home.view

                AboutPage ->
                    About.view

                ContactPage ->
                    Contact.view

                FaqPage ->
                    Faq.view

                NeedHelpPage ->
                    NeedHelp.view

                LegalsPage ->
                    Legals.view

                MyCommandsPage ->
                    MyCommands.view

                MyInformationsPage ->
                    MyInformations.view

                MyAccountPage ->
                    MyAccount.view

                ProductPage productId ->
                    Product.view productId

                _ ->
                    default
    in
    div
        [ class "app-main container" ]
        (routedView model)
