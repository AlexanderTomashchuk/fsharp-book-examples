namespace Unit8.Lesson37.WebApi

open System
open Unit8.Lesson37.Suave.Domain

//module AnimalsRepository =
//  let private all =
//    async {
//      return
//        [ { Name = "Fido"; Species = "Dog" }
//          { Name = "Felix"; Species = "Cat" } ]
//    }
//
//  let getAll () = all
//
//  let getAnimal name =
//    async {
//      match name with
//      | name when name |> Seq.exists Char.IsDigit -> return Error "Name should not contain any digits"
//      | _ ->
//        let! all = getAll ()
//        return all |> List.tryFind (fun a -> a.Name = name) |> Ok
//    }
module AnimalsRepository =
  let private all =
    [ { Name = "Fido"; Species = "Dog" }
      { Name = "Felix"; Species = "Cat" } ]

  let getAll() = all |> Seq.ofList

  let getAnimal name =
    match name with
    | name when name |> Seq.exists Char.IsDigit -> Error "Name should not contain any digits"
    | _ -> all |> List.tryFind (fun a -> a.Name = name) |> Ok
