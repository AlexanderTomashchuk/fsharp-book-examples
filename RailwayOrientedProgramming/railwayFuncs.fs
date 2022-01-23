module RailwayOrientedProgramming.RailwayFunctions

type Result<'TSuccess, 'TFailure> =
  | Success of 'TSuccess
  | Failure of 'TFailure

let succeed x = Success x

let fail x = Failure x

let either successFunc failureFunc twoTrackInput =
  match twoTrackInput with
  | Success s -> successFunc s
  | Failure f -> failureFunc f

let bind f =
  either f fail

let (>>=) x f =
  bind f x

let (>=>) s1 s2 =
  s1 >> bind s2

let switch f =
  f >> succeed

let map f =
  either (f >> succeed) fail

let tee f x =
  f x; x
  
let tryCatch f exnHandler x =
  try
    f x |> succeed
  with
  | ex -> exnHandler ex |> fail

let doubleMap successFunc failureFunc =
  either (successFunc >> succeed) (failureFunc >> fail)

//('a -> 'b -> 'c) -> ('d -> 'd -> 'd) -> ('e -> Result<'a,'d>) -> ('e -> Result<'b,'d>) -> 'e -> Result<'c,'d>
let plus addSuccess addFailure switch1 switch2 x =
  match (switch1 x), (switch2 x) with
  | Success s1, Success s2 -> addSuccess s1 s2 |> succeed
  | Failure f1, Success _ -> f1 |> fail
  | Success _, Failure f2 -> f2 |> fail
  | Failure f1, Failure f2 -> addFailure f1 f2 |> fail
