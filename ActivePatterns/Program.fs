//0 F# has a special type of pattern matching called “active patterns” where the pattern can be parsed or detected dynamically.
open System

let (|Int|_|) str =
  match Int32.TryParse(str: string) with
  | true, int -> Some(int)
  | _ -> None

let (|Bool|_|) str =
  match Boolean.TryParse(str: string) with
  | true, bool -> Some(bool)
  | _ -> None

let testParse str =
  match str with
  | Int i -> printfn "The value is an int '%i'" i
  | Bool b -> printfn "The value is a bool '%b'" b
  | _ -> printfn "The value '%s' is something else" str

testParse "12"
testParse "true"
testParse "abc"

//1
open System.Text.RegularExpressions
let (|FirstRegexGroup|_|) pattern input =
  let m = Regex.Match(input, pattern)
  if m.Success then Some m.Groups.[1].Value else None

let testRegex str =
  match str with
  | FirstRegexGroup "http://(.*?)/(.*)" host -> printfn "The value is an url and the host is %s" host
  | FirstRegexGroup ".*?@(.*)" host -> printfn "The value is an email and the host is %s" host
  | _ -> printfn "The value '%s' is something else" str

testRegex "http://google.com/test"
testRegex  "alice@gmail.com"

//2
let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd

let testNumber input =
  match input with
  | Even -> printfn "%d is even" input
  | Odd -> printfn "%d is odd" input

testNumber 7
testNumber 12
testNumber 33

//3 decompose data types in multiple ways, such as when the same underlying data has various possible representations
open System.Drawing
let (|RGB|) (col: Color) = (col.R, col.G, col.B)

let (|HSB|) (col: Color) =
  (col.GetHue(), col.GetSaturation(), col.GetBrightness())

let printRGB (col: Color) =
  match col with
  | RGB (r, g, b) -> printfn "Red: %d; Green: %d; Blue: %d;" r g b

let printHSB (col: Color) =
  match col with
  | HSB (h, s, b) -> printfn "Hue: %f; Saturation: %f; Brightness: %f" h s b

let printAll col colorString =
  printfn "%s" colorString
  printRGB col
  printHSB col

printAll Color.Red "Red"
printAll Color.Black "Black"
printAll Color.White "White"
printAll Color.Gray "Gray"
printAll Color.BlanchedAlmond "BlanchedAlmond"
