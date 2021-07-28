module Checks

open Types
open Xunit
open MesJolisCotillons.Contracts.Responses

let checkHttpStatus (expectedStatus:HttpStatus) responseItem  =
    let expecteStatusInt = int expectedStatus
    Assert.Equal(responseItem.httpResponse.StatusCode, expecteStatusInt)
    responseItem

let ``Where Success should be``<'TRes when 'TRes:> ResponseBase> expectedResult (responseItem:'TRes)  = 
    Assert.Equal(responseItem.Success, expectedResult)
    responseItem

let checkMessage (expectedMessage:string) (actualMessage:string)  = 
    Assert.Equal(expectedMessage, actualMessage)

/// <summary>
/// Pick up the first message from Response.Messages and compare it with expected message.
/// </summary>
let ``And message should be``<'TRes when 'TRes:> ResponseBase> (expectedMessage:string) (responseItem:'TRes) = 
    let actualMessage = Helpers.getFirstMessage responseItem.Messages
    checkMessage actualMessage expectedMessage
    responseItem

/// <summary>
/// Check count messages from Response Item.
/// </summary>
let ``Where messages count should be``<'TRes when 'TRes:> ResponseBase> (countMessages:int) (responseItem:'TRes) = 
    Assert.Equal(responseItem.Messages.Count, countMessages)
    responseItem

