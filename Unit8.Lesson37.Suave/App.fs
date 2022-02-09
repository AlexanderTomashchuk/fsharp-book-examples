open System
open System.Threading
open Microsoft.FSharp.Control
open Suave
open Unit8.Lesson37.Suave.Rest
open Unit8.Lesson37.WebApi

//let setJsonMimeType =
//  setMimeType "application/json; charset=utf-8"
//
//let getAllAnimalsHandler ctx =
//  async {
//    let! animals = AnimalsRepository.getAll ()
//    let jsonResponse = animals |> JsonSerializer.Serialize
//    return! OK jsonResponse ctx
//  }
//
//let getAnimalByName name =
//  fun (ctx: HttpContext) ->
//    async {
//      let! animal = name |> AnimalsRepository.getAnimal
//      let test = animal |> asResponse ctx
//      return test
//    }

//let app =
//  choose [ GET
//           >=> choose [ path "/animals"
//                        >=> getAllAnimalsHandler
//                        >=> setJsonMimeType
//                        pathScan "/animals/%s" (fun name -> getAnimalByName name) ]
//           NOT_FOUND "Found no handlers" ]

[<EntryPoint>]
let main argv =
  let cts = new CancellationTokenSource()

  let config =
    { defaultConfig with
        cancellationToken = cts.Token }

  let animalsWebPart = rest "animals" { GetAll = AnimalsRepository.getAll }
  let listening, server = startWebServerAsync config animalsWebPart

  Async.Start(server, cts.Token)
  printfn "Make requests now"
  Console.ReadKey true |> ignore

  cts.Cancel()

  0
