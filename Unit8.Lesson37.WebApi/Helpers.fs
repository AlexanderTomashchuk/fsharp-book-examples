namespace Unit8.Lesson37.WebApi

open Microsoft.AspNetCore.Mvc

[<AutoOpen>]
module Helpers =
  let asResponse result =
    match result with
    | Error errorValue -> ObjectResult(errorValue, StatusCode = 400)
    | Ok resultValue ->
      match resultValue with
      | Some value -> ObjectResult(value)
      | None -> ObjectResult("", StatusCode = 404)
