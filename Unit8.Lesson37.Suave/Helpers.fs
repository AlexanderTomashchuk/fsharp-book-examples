namespace Unit8.Lesson37.WebApi

open Suave
open Suave.Json

[<AutoOpen>]
module Helpers =
  let asResponse result (ctx: HttpContext) =
    match result with
    | Error errorValue -> errorValue ctx |> RequestErrors.BAD_REQUEST
//    | Ok resultValue ->
//      match resultValue with
//      | Some value -> value |> toJson |> Successful.ok
//      | None -> "" |> RequestErrors.NOT_FOUND
