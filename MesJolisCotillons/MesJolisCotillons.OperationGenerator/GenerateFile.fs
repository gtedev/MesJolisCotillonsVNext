module GenerateFile

open System.IO

let private createDirectoryIfNotExist folderPath = 
    match Directory.Exists(folderPath) with
    | false -> Directory.CreateDirectory(folderPath) |> ignore
    | _ -> ()
  
let private replaceInTemplateText (sourceText:string) (templateKey:string) (replaceWith:string) = 
    sourceText.Replace(templateKey, replaceWith)

let foldReplaceText accumulatedTextReplaced replacementExpr = 
    replaceInTemplateText accumulatedTextReplaced (fst replacementExpr) (snd replacementExpr)
  
 /// <summary>
 /// Generate file by taking a template, and replacing template text by values provided in replacements sequence
 /// </summary>
 /// <param name="sourceFilePath">Template file</param>
 /// <param name="outputFolderPath">Output folder where the file will be generated</param>
 /// <param name="outputFileName">Name of the output file</param>
 /// <param name="replacements">Sequence of TemplateReplacement, in which a TemplateReplacement specifies a templateKey to replace with a specific text</param>
 /// <returns></returns>
let generateFile sourceFilePath outputFolderPath  outputFileName  (replacements: seq<string*string>) =

    createDirectoryIfNotExist outputFolderPath
    let sourceTemplateText = File.ReadAllText(sourceFilePath)

    let textReplaced = 
      replacements
      |> Seq.fold foldReplaceText sourceTemplateText

    File.WriteAllText(outputFolderPath + "/" +  outputFileName , textReplaced)
