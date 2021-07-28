module IntegrationTestsMiddleware

 open FSharp.Data
 open Newtonsoft.Json
 open JsonNet.PrivateSettersContractResolvers
 open Types
 

 let createHttpRequest  url =
    { url = url ; httpMethod = HttpMethod.GET; httpBody = None; headers = None; queryParameters = None}


 let createMesJolisCotillonsRequest  url =
    let requestUrl = MjcApi.Config.BaseUrl + url
    createHttpRequest requestUrl


 let withHttpMethod  httpMethod requestParams =  
    { requestParams with httpMethod = httpMethod }


 /// <summary>
 /// Set POST request with application/json headers.
 /// </summary>
 let withHttpPostMethod requestParams =  
   { requestParams with httpMethod = HttpMethod.POST; headers = Some (seq { ("Content-Type","application/json") })}
     

 let withBodyRequest<'TReq> (request:'TReq) requestParams =
   let serializedRequest = JsonConvert.SerializeObject(request)
   { requestParams with httpBody = Some (TextRequest serializedRequest) }

 let withQueryParameters (parameters: (string * string) list) requestParams =
    { requestParams with queryParameters = Some (parameters) }


 let send<'TRes> requestParams =
    let url = requestParams.url
    let method = requestParams.httpMethod.ToString()
   
    let httpResponse =
       match requestParams.httpBody, requestParams.headers, requestParams.queryParameters with
       | Some body, Some header, None -> 
            Http.Request(url, httpMethod = method, body = body, headers= header, silentHttpErrors = false)
       | None, None, Some queryParameters -> 
            Http.Request(url, httpMethod = method, query=queryParameters, silentHttpErrors = false)
       | _, _, _  -> 
            Http.Request(url, httpMethod = method, silentHttpErrors = false)

    let responseBodyString = 
       match httpResponse.Body with
       | Text body -> body.ToString()
       | _ -> ""

    let serializer = new JsonSerializerSettings(ContractResolver = new PrivateSetterContractResolver())
    let deserializedItem = JsonConvert.DeserializeObject<'TRes>(responseBodyString, serializer)
    
    let responseItem = { Response = deserializedItem; httpResponse = httpResponse }
    responseItem


 let toResponse responseItem =
    responseItem.Response
