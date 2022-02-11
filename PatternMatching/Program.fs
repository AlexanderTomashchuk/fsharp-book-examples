#nowarn "20"

//https://fsharpforfunandprofit.com/posts/match-expression/#types-of-patterns

open Microsoft.FSharp.Core

let y =
  match (1, 0) with
  | (1, x) -> printf "x=%A" x
  | (z, x) -> printf "x=%A" 0

let y2 =
  match (1, 0) with
  // OR  -- same as multiple cases on one line
  | 2, x
  | 3, x
  | 4, x -> printfn "x=%A" x

  // AND  -- must match both patterns at once
  // Note only a single "&" is used
  | (2, x) & (_, 1) -> printfn "x=%A" x

type Choices =
  | A
  | B
  | C
  | D

let x =
  match A with
  | A
  | B
  | C -> "a or b or c"
  | D -> "d"

let y3 =
  match [ 1; 2; 3 ] with
  // binding to explicit positions
  // square brackets used!
  | [ 1; x; y ] -> printfn "x=%A y=%A" x y
  // binding to head::tail.
  // no square brackets used!
  | 1 :: tail -> printfn "tail=%A" tail
  // empty list
  | [] -> printfn "empty"

let rec loopAndPrint list =
  match list with
  | [] -> printfn ""
  | head :: tail ->
    printfn "%A" head
    loopAndPrint tail

loopAndPrint [ 1 .. 10 ]

let rec loopAndSum aList sumSoFar =
  match aList with
  | [] -> sumSoFar
  | head :: tail ->
    let sum = sumSoFar + head
    loopAndSum tail sum

([ 1; 2; 3; 4; 5 ], 0) ||> loopAndSum

let aTuple = (1, 2)

match aTuple with
| (1, _) -> printfn "first part is 1"
| (_, 2) -> printfn "second part is 2"

type Person = { First: string; Last: string }
let person = { First = "Alex"; Last = "Tom" }

match person with
| { First = "Alex" } -> printfn "Gav"
| _ -> printfn "Not Alex"

//union pattern matching
type IntOrBool =
  | I of int
  | B of bool

let intOrBool = I 42
let intOrBool2 = B false

match intOrBool2 with
| I i -> printfn "%d" i
| B b -> printfn "%b" b

//Matching the whole and the part with the “as” keyword
let y4 =
  match (1, 0) with
  | (x, y) as t ->
    printfn "x=%A; y=%A" x y
    printfn "The whole tuple is %A" t

//Matching on subtypes using :?
let x2 = new System.Object()

match x2 with
| :? System.Int32 -> printfn "matched with int"
| :? System.DateTime -> printfn "matched with datetime"
| _ -> printfn "another type"

let detectTypeBoxed v =
  match box v with // used "box v"
  | :? int -> printfn "this is an int"
  | _ -> printfn "something else"

//test
detectTypeBoxed 1
detectTypeBoxed 3.14

let matchOnTwoParameters x y =
  match (x, y) with
  | (1, y) -> printfn "x=1 and y=%A" y
  | (x, 1) -> printfn "x=%A and y=1" x

let elementsAreEqual aTuple =
  match aTuple with
  | (x, y) when x = y -> printfn "both parts are the same"
  | _ -> printfn "both parts are different"

(1, 2) |> elementsAreEqual
(1, 1) |> elementsAreEqual

let makeOrdered aTuple =
  match aTuple with
  | (x, y) when x > y -> (y, x)
  | _ -> aTuple

makeOrdered (1, 2)
makeOrdered (2, 1)

// pattern matching using regular expressions
open System.Text.RegularExpressions

let classifyString aString =
  match aString with
  | x when Regex.Match(x, @".+@.+").Success -> printfn "%s is an email" aString
  // otherwise leave alone
  | _ -> printfn "%s is something else" aString

classifyString "alice@example.com"
classifyString "google.com"

//active patterns

let (|EmailAddress|_|) input =
  let m = Regex.Match(input, @".+@.+")
  if (m.Success) then Some input else None

let classifyString2 aString =
  match aString with
  | EmailAddress x -> printfn "%s is an email" x
  | _ -> printfn "%s is something else" aString

classifyString2 "alice@example.com"
classifyString2 "google.com"

//Exception handling with try..with

let debugMode = false

try
  failwith "fail"
with
| Failure msg when debugMode -> reraise ()
| Failure msg when not debugMode -> printfn "silently logged in production: %s" msg

let times6 x = x * 6

let isAnswerToEverything x =
  match x with
  | 42 -> (x, true)
  | _ -> (x, false)

// the function can be used for chaining or composition
[ 1 .. 10 ]
|> List.map (times6 >> isAnswerToEverything)

open Microsoft.FSharp.Collections
let loopAndSum1 (aList: int list) = List.sum aList
[ 1 .. 10 ] |> loopAndSum1

let loopAndSum2 (aList: int list) = List.reduce (+) aList
[ 1 .. 10 ] |> loopAndSum2

let loopAndSum3 (aList: int list) =
  List.fold (fun acc i -> acc + i) 0 aList

[ 1 .. 10 ] |> loopAndSum3

// unnecessary to implement this explicitly
let addOneIfValid optionalInt =
  match optionalInt with
  | Some i -> Some(i + 1)
  | None -> None

Some 42 |> addOneIfValid

let addOneIfValid2 optionalInt =
  optionalInt |> Option.map (fun x -> x + 1)

Some 42 |> addOneIfValid2
None |> addOneIfValid2

//!!!Creating “fold” functions to hide matching logic

type TemperatureType = F of float | C of float

module Temperature =
  let fold fahrenheitFunction celsiusFunction aTemp =
    match aTemp with
    | F f -> fahrenheitFunction f
    | C c -> celsiusFunction c

let fFever tempF =
    if tempF > 100.0 then "Fever!" else "OK"

let cFever tempC =
    if tempC > 38.0 then "Fever!" else "OK"

open Temperature

let isFever aTemp = aTemp |> fold fFever cFever

let normalTemp = C 37.0
let result1 = isFever normalTemp

let highTemp = F 103.1
let result2 = isFever highTemp

let fConversion tempF =
    let convertedValue = (tempF - 32.0) / 1.8
    TemperatureType.C convertedValue    //wrapped in type

let cConversion tempC =
    let convertedValue = (tempC * 1.8) + 32.0
    TemperatureType.F convertedValue    //wrapped in type

let convert aTemp = Temperature.fold fConversion cConversion aTemp

let c20 = C 20.0
let resultInF = convert c20

let f75 = F 75.0
let resultInC = convert f75

let resultInC2 = C 20.0 |> convert |> convert
