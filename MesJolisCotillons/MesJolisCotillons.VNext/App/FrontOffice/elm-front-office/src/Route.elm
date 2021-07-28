module Route exposing (Route(..), parseUrl)

import Url exposing (Url)
import Url.Parser exposing (..)


type Route
    = NotFound
    | HomePage
    | AboutPage
    | ContactPage
    | FaqPage
    | NeedHelpPage
    | LegalsPage
    | MyCommandsPage
    | MyInformationsPage
    | MyAccountPage
    | ProductPage Int


parseUrl : Url -> Route
parseUrl url =
    case parse matchRoute url of
        Just route ->
            route

        Nothing ->
            NotFound


matchRoute : Parser (Route -> a) a
matchRoute =
    oneOf
        [ map HomePage top
        , map AboutPage (s "a-propos")
        , map ContactPage (s "contact")
        , map FaqPage (s "faq")
        , map NeedHelpPage (s "besoin-d-aides")
        , map LegalsPage (s "mentions-legales")
        , map ContactPage (s "contact")
        , map MyCommandsPage (s "mes-commandes")
        , map MyInformationsPage (s "mes-informations")
        , map MyAccountPage (s "mon-compte")
        , map ProductPage (s "product" </> int)
        ]
