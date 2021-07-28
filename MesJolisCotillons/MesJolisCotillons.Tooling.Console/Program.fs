// Learn more about F# at http://fsharp.org

open Types

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    let command: GenerateFilesCommand = {
          operationName =  "Herve";
          relativeFolder=  "Herve/Create";
          hasAdapter=  false;
          hasValidations=  false;
          hasExecutor=  true;
          useCustomResponseBuilder=  true
    }
    
    let logger (msg:string) =  
      ()
    
    OperationGenerator.GenerateOperation.generateAllOperationClasses command logger


    0 // return an integer exit code
