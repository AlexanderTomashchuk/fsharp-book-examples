namespace Unit8.Lesson37.Suave.Rest

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave
open Suave.Filters
open Suave.Operators

[<AutoOpen>]
module Restful =
  //todo: finish later https://blog.tamizhvendan.in/blog/2015/06/11/building-rest-api-in-fsharp-using-suave/
  type RestResource<'a> = { GetAll: unit -> seq<'a> }

  let JSON v : WebPart =
    let jsonSerializerSettings = JsonSerializerSettings()
    jsonSerializerSettings.ContractResolver <- CamelCasePropertyNamesContractResolver()

    JsonConvert.SerializeObject(v, jsonSerializerSettings)
    |> Successful.OK
    >=> Writers.setMimeType "application/json; charset=utf-8"

  let rest resourceName resource : WebPart =
    let resourcePath = "/" + resourceName

    let getAll =
      warbler (fun _ -> resource.GetAll() |> JSON)

    path resourcePath >=> GET >=> getAll
