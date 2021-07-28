module Types

type GenerateFilesCommand = {
    operationName : string
    relativeFolder: string
    hasAdapter: bool
    hasValidations: bool
    hasExecutor: bool
    useCustomResponseBuilder: bool
}

type FileConfig = {
    FileBaseName: string
    TemplatePath: string
    OutputFolder: string
    IsInterface:    bool
    ReplacementExpressions: seq<string*string>
}