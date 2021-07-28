module Types

open FSharp.Data

type HttpMethod = 
 | GET = 0 
 | POST = 1 
 | DELETE = 2

type HttpStatus = 
 | Success = 200 
 | Accepted = 201 
 | BadRequest = 400
 | Unauthorized = 401
 | Forbidden = 403
 | NotFound = 404
 | InternalServerError = 500
 

type RequestParameters = {
   headers: Option<seq<string*string>>
   httpBody: Option<HttpRequestBody>
   url: string
   httpMethod: HttpMethod 
   queryParameters: Option<(string*string) list>
}

type ResponseItem<'TRes> = {
   Response: 'TRes;
   httpResponse: HttpResponse 
}

