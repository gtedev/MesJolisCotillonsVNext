module GenerationFileConfig

open FSharp.Data

type GeneratorFileConfig = JsonProvider<"appsettings.json">

let GetFileConfig() = 
    GeneratorFileConfig.GetSample()

let GetFullTemplateRootPath() = 
    let config = GetFileConfig()
    config.SolutionRootPath + "\\" + config.TemplateRootPath

let GetFileConfigNode (templatePath: string) =
    GeneratorFileConfig.GetSample().Files
    |> Seq.filter (fun x -> x.TemplatePath = templatePath)
    |> Seq.head

