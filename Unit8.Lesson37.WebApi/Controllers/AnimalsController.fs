namespace Unit8.Lesson37.WebApi.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Unit8.Lesson37.WebApi

[<ApiController>]
[<Route("animals")>]
type AnimalsController(logger: ILogger<AnimalsController>) =
  inherit ControllerBase()

  [<HttpGet("{name}")>]
  member _.Get(name) = async {
    let! result = AnimalsRepository.getAnimal name 
    return result |> asResponse
  }
