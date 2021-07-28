namespace OperationGenerator

open System
open Types

module GenerateOperation = 

    open GenerateFile

    let private ToInterfaceName name = 
        "I" + name

    let private ToFullPath path = 
        let config =  GenerationFileConfig.GetFileConfig()
        config.SolutionRootPath + "\\" + path

    let private ToFullTemplatePath templatePath = 
        let config =  GenerationFileConfig.GetFileConfig()
        config.SolutionRootPath + "\\" + config.TemplateRootPath  + "\\" + templatePath

    let private getBasicReplacementExpressions operationName (relativeFolder:string) = 
        // handle case when one and two slashes are provided...should be handled with regex expression...
        let arraySlash = [@"/";@"//";@"\";@"\\"]

        let nameSpace = 
         arraySlash
         |> Seq.fold (fun (acc:string) x -> acc.Replace(x, ".") ) relativeFolder

        let replacments = seq {
            yield ( "TemplateOperationName" , operationName)
            yield ("TemplateNamespace" , nameSpace)
        }

        replacments

    let private getOutputFileName operationName fileConfigName isInterface = 
        let outputFileName = operationName + fileConfigName + ".cs"
        match isInterface with
        | true -> outputFileName |> ToInterfaceName
        | false -> outputFileName 

    let private generateFileBase operationName relativeFolder logger (fileConfig: FileConfig) =
         let sourcePath = 
          fileConfig.TemplatePath 
          |> ToFullTemplatePath

         let outputFolderPath = 
          fileConfig.OutputFolder + "\\" + relativeFolder 
          |> ToFullPath
     
         let outputFileName = getOutputFileName operationName fileConfig.FileBaseName fileConfig.IsInterface

         generateFile sourcePath outputFolderPath outputFileName fileConfig.ReplacementExpressions
         logger (outputFileName + " file generated..." + Environment.NewLine)
    
    let private getExceptAdapterFiles hasAdapter = 
        let files = GenerationFileConfig.GetFileConfig().Files

        let result = 
            match hasAdapter with
             | false -> files |> Seq.filter (fun x ->  Array.contains "adp" x.Flags)
             | true -> seq[]

        result

    let private getExceptCommandFiles hasAdapter = 
         let files = GenerationFileConfig.GetFileConfig().Files

         let result = 
             match hasAdapter with
             | true -> files |> Seq.filter (fun x ->  Array.contains "cmd" x.Flags && Array.contains "withoutadp" x.Flags  )
             | false -> files |> Seq.filter (fun x ->  Array.contains "adp" x.Flags && Array.contains "cmd" x.Flags )
      
         result

    let private getExceptValidationFiles hasValidations = 
         let files = GenerationFileConfig.GetFileConfig().Files
         
         let result = 
             match hasValidations with
             | true -> seq[] 
             | false -> files |> Seq.filter (fun x ->  Array.contains "val" x.Flags)
         
         result

    let private getExceptExecutorFiles hasExecutor = 
        let files = GenerationFileConfig.GetFileConfig().Files

        let result = 
            match hasExecutor with
            | false -> files |> Seq.filter (fun x ->  Array.contains "exec" x.Flags)
            | true -> seq[]

        result

    let private getExceptResponseFiles useCustomResponseBuilder = 
         let files = GenerationFileConfig.GetFileConfig().Files

         let result = 
             match useCustomResponseBuilder with
             | false -> files |> Seq.filter (fun x ->  Array.contains "customResponse" x.Flags)
             | true -> files |> Seq.filter (fun x ->  Array.contains "defaultResponse" x.Flags)

         result

    let private getOperationExpressionReplacements (command: GenerateFilesCommand) = 
        let adapterExpressions = 
            match command.hasAdapter with 
            | true ->   Seq.empty
            | false -> 
                seq { 
                        yield "using MesJolisCotillons.Adapters.TemplateNamespace;\r\n    ", String.Empty
                        yield "private readonly ITemplateOperationNameAdapter adapter;\r\n        ", String.Empty 
                        yield "ITemplateOperationNameAdapter adapter,\r\n            ", String.Empty 
                        yield "this.adapter = adapter;\r\n            ", String.Empty
                        yield ".AddAdapter(this.adapter, request)\r\n                    ", String.Empty
                    }
        
        let isCommandNotLast = command.hasValidations ||  command.hasExecutor  ||  command.useCustomResponseBuilder 
        let commandCharacter = if isCommandNotLast then "," else ")"
        let commandExpressions = 
            match command.hasAdapter with 
            | true ->   Seq.empty
            | false -> 
                seq { 
                        yield ".AddCommandBuilder(this.commandBuilder)", ".AddCommandBuilder(this.commandBuilder, request)"
                        yield "ITemplateOperationNameCommandBuilder commandBuilder,", "ITemplateOperationNameCommandBuilder commandBuilder" + commandCharacter
                    }

        let isValidationNotLast = command.hasExecutor  ||  command.useCustomResponseBuilder 
        let validationCharacter = if isValidationNotLast then "," else ")"
        let validationExpressions = 
            match command.hasValidations with 
            | true ->   
                seq { 
                          yield "ITemplateOperationNameValidationBuilder validationBuilder,", "ITemplateOperationNameValidationBuilder validationBuilder" + validationCharacter
                    }
            | false -> 
                seq { 
                        yield "using MesJolisCotillons.Validation.Builders.TemplateNamespace;\r\n    ", String.Empty
                        yield "private readonly ITemplateOperationNameValidationBuilder validationBuilder;\r\n        ", String.Empty
                        yield "ITemplateOperationNameValidationBuilder validationBuilder,\r\n            ", String.Empty
                        yield "this.validationBuilder = validationBuilder;\r\n            ", String.Empty
                        yield ".AddValidationBuilder(this.validationBuilder)\r\n                    ", String.Empty
                    }

        let isExecutorNotLast = command.useCustomResponseBuilder 
        let executorCharacter = if isExecutorNotLast then "," else ")"
        let executorExpressions = 
            match command.hasExecutor with 
            | true ->  
                seq { 
                           yield "ITemplateOperationNameExecutorBuilder executorBuilder,", "ITemplateOperationNameExecutorBuilder executorBuilder" + executorCharacter
                    }
            | false -> 
                seq { 
                        yield "using MesJolisCotillons.Executors.Builders.TemplateNamespace;\r\n    ", String.Empty
                        yield "private readonly ITemplateOperationNameExecutorBuilder executorBuilder;\r\n        ", String.Empty
                        yield "ITemplateOperationNameExecutorBuilder executorBuilder,\r\n            ", String.Empty
                        yield "this.executorBuilder = executorBuilder;\r\n            ", String.Empty
                        yield ".AddExecutorBuilder(this.executorBuilder)\r\n                    ", String.Empty
                    }

        let responseExpressions = 
            match command.useCustomResponseBuilder with 
            | true ->   Seq.empty
            | false -> 
                seq { 
                        yield "using MesJolisCotillons.Response.Builders.TemplateNamespace;\r\n    ", String.Empty
                        yield "private readonly ITemplateOperationNameResponseBuilder responseBuilder;\r\n", String.Empty
                        yield "ITemplateOperationNameResponseBuilder responseBuilder)\r\n", String.Empty
                        yield "this.responseBuilder = responseBuilder;\r\n", String.Empty
                        yield ".AddResponseBuilder(this.responseBuilder)\r\n                    ", ".AddDefaultResponseBuilder()\r\n                    "
                    }

        let expressions = getBasicReplacementExpressions command.operationName command.relativeFolder
        
        expressions
        |> Seq.append adapterExpressions
        |> Seq.append commandExpressions
        |> Seq.append validationExpressions
        |> Seq.append executorExpressions
        |> Seq.append responseExpressions

    /// <summary>
    /// Generate Operation classes for MesJolisCotillons project.
    /// </summary>
    /// <param name="operationName">Operation name. No need to suffix it with "Operation", as it will be added automatically.</param>
    /// <param name="relativeFolder">Relative path folder that will be used under each project to generate new classes.</param>
    /// <param name="hasValidations">True for generate Validation Builder classes, that will be used in the Operation.</param>
    /// <param name="useCustomResponseBuilder">True for generate classes for Custom Response Bulder. With False, Operation will use Default Response Builder.</param>
    /// <param name="logger">Sequence of TemplateReplacement, in which a TemplateReplacement specifies a templateKey to replace with a specific text.</param>
    /// <returns></returns>
    let generateAllOperationClasses (command: GenerateFilesCommand) logger = 

        // generate non operation files
        let nonOperationFiles = 
         GenerationFileConfig.GetFileConfig().Files
         |> Seq.filter (function f ->  not (Array.contains "op" f.Flags))
         |> Seq.except (getExceptAdapterFiles command.hasAdapter)
         |> Seq.except (getExceptCommandFiles command.hasAdapter)
         |> Seq.except (getExceptValidationFiles command.hasValidations)
         |> Seq.except (getExceptExecutorFiles command.hasExecutor)
         |> Seq.except (getExceptResponseFiles command.useCustomResponseBuilder)

        let replacements = 
            getBasicReplacementExpressions command.operationName command.relativeFolder

        nonOperationFiles
        |> Seq.map (fun x -> 
            { 
                FileBaseName = x.FileBaseName; 
                TemplatePath = x.TemplatePath; 
                OutputFolder = x.OutputFolder; 
                IsInterface = x.IsInterface ;
                ReplacementExpressions = replacements;
             })
        |> Seq.iter (fun fileConfig -> generateFileBase command.operationName command.relativeFolder logger fileConfig)


        // generate operation file
        let operationReplacements = getOperationExpressionReplacements  command
        let getOperationTemplate = GenerationFileConfig.GetFileConfigNode "TemplateOperation'1.cs"

        let operationFileConfig = { 
            FileBaseName = getOperationTemplate.FileBaseName; 
            TemplatePath = getOperationTemplate.TemplatePath; 
            OutputFolder = getOperationTemplate.OutputFolder; 
            IsInterface = getOperationTemplate.IsInterface ;
            ReplacementExpressions = operationReplacements;
         }

        generateFileBase command.operationName command.relativeFolder logger operationFileConfig
        
        let countFiles = (Seq.length nonOperationFiles) + 1
        let countFileMsgTxt = Environment.NewLine + Environment.NewLine + countFiles.ToString() + " file(s) generated..."
        logger countFileMsgTxt