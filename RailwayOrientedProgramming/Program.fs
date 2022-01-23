open Microsoft.FSharp.Core

type Result<'TSuccess, 'TFailure> =
  | Success of 'TSuccess
  | Failure of 'TFailure

type Request = { Name: string; Email: string }

let validateInput input =
  if input.Name = "" then
    Failure "Name must not be blank"
  elif input.Email = "" then
    Failure "Email must not be blank"
  else
    Success input

let bind f result =
  match result with
  | Success s -> f s
  | Failure f -> Failure f

let bindF f =
  fun result ->
    match result with
    | Success s -> f s
    | Failure f -> Failure f

let bindImproved f =
  function
  | Success s -> f s
  | Failure f -> Failure f

let validate1 input =
  if input.Name = "" then
    Failure "Name must not be blank"
  else
    Success input

let validate2 input =
  if input.Name.Length > 50 then
    Failure "Name must not be longer than 50 characters"
  else
    Success input

let validate3 input =
  if input.Email = "" then
    Failure "Email must not be blank"
  else
    Success input

let combinedValidation1 =
  let validate2' = bind validate2
  let validate3' = bind validate3
  validate1 >> validate2' >> validate3'

let combinedValidationImproved input =
  input
  |> validate1
  |> bind validate2
  |> bind validate3

let validationTest =
  { Name = "Alex"
    Email = "alex@gmail.com" }
  |> combinedValidation1
  |> printfn "%A"

let (>>=) result f = bind f result

//let combinedValidation x =
//  x |> validate1 >>= validate2 >>= validate3

//switch composition
let (>=>) switch1 switch2 x =
  match switch1 x with
  | Success s -> switch2 s
  | Failure f -> Failure f
//the same as previous function
//let (>=>) switch1 switch2 =
//  switch1 >> bind switch2

let combinedValidation = validate1 >=> validate2 >=> validate3

let canonicalizeEmail (input: Request) =
  { input with
      Email = input.Email.Trim().ToLower() }

//one-track function to switch function
let switch f x = f x |> Success //Also known as a "lift" in some contexts.

let usecase =
  validate1
  >=> validate2
  >=> validate3
  >=> switch canonicalizeEmail

let goodInput =
  { Name = "Alice"
    Email = "UPPERCASE   " }

goodInput |> usecase |> printfn "%A"

let badInput = { Name = ""; Email = "UPPERCASE   " }
badInput |> usecase |> printfn "%A"

//two-track functions from one-track functions
let map f rf = //Also known as a "lift" in some contexts.
  match rf with
  | Success s -> Success(f s)
  | Failure f -> Failure f

let usecase2 =
  validate1 >=> validate2 >=> validate3
  >> map canonicalizeEmail

//dead-end functions to two-track functions
let tee f x = //Also known as tap
  f x |> ignore
  x

let updateDatabase (input: Request) =
  //failwith "Update db error"
  ()

let usecase3 =
  validate1
  >=> validate2
  >=> validate3
  >=> switch canonicalizeEmail
  >=> switch (tee updateDatabase)

goodInput |> usecase3 |> printfn "%A"

let tryCatch f x =
  try
    f x |> Success
  with
  | ex -> Failure ex.Message

let usecase4 =
  validate1
  >=> validate2
  >=> validate3
  >=> switch canonicalizeEmail
  >=> tryCatch updateDatabase

goodInput |> usecase4 |> printfn "%A"

let doubleMap successFunc failureFunc twoTrackInput = //Also known as bimap
  match twoTrackInput with
  | Success s -> Success(successFunc s)
  | Failure f -> Failure(failureFunc f)

let mapSimpler successFunc = doubleMap successFunc id

let log twoTrackInput =
  let success x =
    printfn "DEBUG. Success so far: %A" x
    x

  let failure x =
    printfn "ERROR. %A" x
    x

  doubleMap success failure twoTrackInput

let usecase5 =
  validate1 >=> validate2 >=> validate3 >> log
  >=> switch canonicalizeEmail
  >=> tryCatch (tee updateDatabase)
  >> log

let goodInput2 = { Name = "Alex"; Email = "  QWERTY  " }
goodInput2 |> usecase5 |> ignore

let badInput2 = { Name = ""; Email = "  QWERTY  " }
badInput2 |> usecase5 |> ignore

let succeed x = Success x //in other contexts, this might also be called return or pure.

let fail x = Failure x

let plus addSuccess addFailure switch1 switch2 x = //Also known as ++ and <+> in other contexts.
  match (switch1 x), (switch2 x) with
  | Success s1, Success s2 -> Success(addSuccess s1 s2)
  | Failure f1, Success _ -> Failure f1
  | Success _, Failure f2 -> Failure f2
  | Failure f1, Failure f2 -> Failure(addFailure f1 f2)

let (&&&) v1 v2 =
  let addSuccess r1 _ = r1
  let addFailure s1 s2 = s1 + ";" + s2
  plus addSuccess addFailure v1 v2

let combinedValidation2 = validate1 &&& validate2 &&& validate3

let input11 = { Name = ""; Email = "" }

input11
|> combinedValidation2
|> printfn "Result1 = %A"

let input21 = { Name = "Alice"; Email = "" }

input21
|> combinedValidation2
|> printfn "Result1 = %A"

let input31 = { Name = "Alice"; Email = "good" }

input31
|> combinedValidation2
|> printfn "Result1 = %A"

let usecase6 =
  combinedValidation2
  >=> switch canonicalizeEmail
  >=> tryCatch (tee updateDatabase)
  >> log

let input41 = { Name = "Alice"; Email = "  TTEETT  " }
input41 |> usecase6 |> ignore

//You might be asking, can we create a way of OR-ing validation functions as well? That
//is, the overall result is valid if either part is valid? The answer is yes, of course.
//Try it! I suggest that you use the symbol ||| for this.

let (|||) v1 v2 =
  let addSuccess r1 _ = r1
  let addFailure s1 _ = s1
  plus addSuccess addFailure v1 v2

let combinedValidation3 = validate1 ||| validate2 ||| validate3

let input12 = { Name = ""; Email = "" }

input12
|> combinedValidation3
|> printfn "Result1 = %A"

let input22 = { Name = "Alice"; Email = "" }

input22
|> combinedValidation3
|> printfn "Result1 = %A"

let input32 = { Name = "Alice"; Email = "good" }

input32
|> combinedValidation3
|> printfn "Result1 = %A"

let usecase7 =
  combinedValidation3
  >=> switch canonicalizeEmail
  >=> tryCatch (tee updateDatabase)
  >> log

let input42 = { Name = "Alice"; Email = "  TTEETT  " }
input42 |> usecase7 |> ignore

//dynamic injection of functions
type Config = { debug: bool }

//example
//let injectableFunction f = if config.debug then f else id

let debugLogger twoTrackInput =
  let success s =
    printfn "DEBUG. Success so far: %A" s
    s

  let fail f =
    printfn "ERROR. %A" f
    f

  doubleMap success fail twoTrackInput

let injectableLogger config =
  if config.debug then debugLogger else id

let usecase8 config =
  combinedValidation2
  >=> switch canonicalizeEmail
  >=> tryCatch (tee updateDatabase)
  >> injectableLogger config

let input13 = { Name = "Alice"; Email = "  EETT  " }

let releaseConfig = {debug = false}
input13 |> usecase8 releaseConfig |> ignore

let debugConfig = {debug = true}
input13 |> usecase8 debugConfig |> ignore




