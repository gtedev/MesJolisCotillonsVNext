module MjcApi

open FSharp.Data

type MesJolisCotillonApiConfig = JsonProvider<"appsettings.json">

let Config =
    MesJolisCotillonApiConfig.GetSample()

let Api = 
    MesJolisCotillonApiConfig.GetSample().Api

