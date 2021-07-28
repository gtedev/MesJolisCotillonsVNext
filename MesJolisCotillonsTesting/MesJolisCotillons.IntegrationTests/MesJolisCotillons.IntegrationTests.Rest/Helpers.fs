module Helpers


  let getFirstMessage (messages: System.Collections.Generic.List<string>) = 
    messages |> Seq.toList |> List.head

  ////let extractResponse<'TRes> (responseItem: ResponseItem<'TRes>) = 
  //// responseItem.Response