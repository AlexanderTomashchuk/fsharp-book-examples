namespace Unit8.Lesson37.WebApi

open System
open Microsoft.AspNetCore.Mvc
open Unit8.Lesson37.WebApi.Domain

module AnimalsRepository =
  let private all =
    async {
      return
        [ { Name = "Fido"; Species = "Dog" }
          { Name = "Felix"; Species = "Cat" } ]
    }

  let getAll () = all

  let getAnimal name =
    async {
      match name with
      | name when name |> Seq.exists Char.IsDigit -> return Error "Name should not contain any digits"
      | _ ->
        let! all = getAll ()
        return all |> List.tryFind (fun a -> a.Name = name) |> Ok
    }
