#nowarn "20"

//https://fsharpforfunandprofit.com/posts/elevated-world/

//the map function
//Common Names: map, fmap, lift, Select
//Common Operators: <$> <!>
//What it does: Lifts a function into the elevated world
//Signature: (a->b) -> E<a> -> E<b>. Alternatively with the parameters reversed: E<a> -> (a->b) -> E<b>

//Examples of map

open Microsoft.FSharp.Control
open Microsoft.FSharp.Core

let mapOption f opt =
  match opt with
  | Some value -> Some(f value)
  | None -> None

let rec mapList f list =
  match list with
  | [] -> []
  | head :: tail -> (f head) :: (mapList f tail)

//define the function in normal world
let add1 x = x + 1

//a function lifted to the world of Options
let add1IfSomething = Option.map add1

//a function lifted to the world of Lists
let add1ToEachElement = List.map add1

//tests
Some 10 |> add1IfSomething
None |> add1IfSomething

[ 1; 2; 3 ] |> add1ToEachElement
[] |> add1ToEachElement

//or
Some 10 |> Option.map add1
None |> Option.map add1

[ 1; 2; 3 ] |> List.map add1
[] |> List.map add1

//the return function
//Common Names: return, pure, unit, yield, point
//Common Operators: None
//What it does: Lifts a single value into the elevated world
//Signature: a -> E<a>

// A value lifted to the world of Options
let returnOption x = Some x

let returnList x = [ x ]

//the apply function
//Common Names: apply, ap
//Common Operators: <*>
//What it does: Unpacks a function wrapped inside an elevated value into a lifted function E<a> -> E<b>
//Signature: E<(a->b)> -> E<a> -> E<b>

module Option =
  // The apply function for Options
  let applyCustom fOpt xOpt =
    match fOpt, xOpt with
    | Some f, Some x -> Some(f x)
    | _ -> None

  let (<*>) = applyCustom
  let (<!>) = Option.map
  let lift2 f x y = f <!> x <*> y
  let lift3 f x y z = f <!> x <*> y <*> z
  let lift4 f x y z q = f <!> x <*> y <*> z <*> q

module List =
  // The apply function for lists
  // [f;g] apply [x;y] becomes [f x; f y; g x; g y]
  let applyCustom (fList: ('a -> 'b) list) (xList: 'a list) =
    [ for f in fList do
        for x in xList do
          yield f x ]

  let (<*>) = applyCustom
  let (<!>) = List.map
  let lift2 f x y = f <!> x <*> y
  let lift3 f x y z = f <!> x <*> y <*> z
  let lift4 f x y z q = f <!> x <*> y <*> z <*> q

//tests
let add x y = x + y

let resultOption =
  let (<*>) = Option.applyCustom
  (Some add) <*> (Some 2) <*> (Some 3) //Some 5

let resultList =
  let (<*>) = List.applyCustom
  [ add ] <*> [ 1; 2 ] <*> [ 10; 20 ] //[11; 21; 12; 22]

let resultOption2 =
  let (<!>) = Option.map
  let (<*>) = Option.applyCustom

  add <!> (Some 2) <*> (Some 3) //Some 5

let resultList2 =
  let (<!>) = List.map
  let (<*>) = List.applyCustom

  add <!> [ 1; 2 ] <*> [ 10; 20 ] //[11; 21; 12; 22]

//the liftN family of functions
//Common Names: lift2, lift3, lift4 and similar
//Common Operators: None
//What it does: Combines two (or three, or four) elevated values using a specified function
//Signature: lift2: (a->b->c) -> E<a> -> E<b> -> E<c>, lift3: (a->b->c->d) -> E<a> -> E<b> -> E<c> -> E<d>, etc.
let addPair x y = x + y

open Option
let addPairOpt x y = addPair <!> x <*> y
//or just
let addPairOpt2 = Option.lift2 addPair

//tests
(Some 2, Some 3) ||> addPairOpt |> Option.get

(Some 2, Some 3)
||> Option.lift2 addPair
|> Option.get

//compiles with errors
(Some 2, None) ||> addPairOpt2 |> Option.get

(Some 2, None)
||> Option.map2 addPair
|> Option.get

let tuple x y = x, y
let combineOpt x y = Option.lift2 tuple x y
let combineList x y = List.lift2 tuple x y

(Some 2, Some 3) ||> combineOpt |> Option.get //(2,3)

([ 1; 2 ], [ 100; 200 ]) ||> combineList //[(1, 100); (1, 200); (2, 100); (2, 200)]

(Some 2, Some 3)
||> combineOpt
|> Option.map (fun (x, y) -> x + y) //Some 5

([ 1; 2 ], [ 100; 200 ])
||> combineList
|> List.map (fun (x, y) -> x + y) //[101; 201; 102; 202]

//the zip function and zipList world
//Common Names: zip, zipWith
//Common Operators: <*> (in the context of ZipList world)
//What it does: Combines two lists (or other enumerables) using a specified function
//Signature: E<(a->b->c)> -> E<a> -> E<b> -> E<c> where E is a list or other enumerable type,
//or E<a> -> E<b> -> E<a,b> for the tuple-combined version.

// alternate "zip" implementation
// [f;g] apply [x;y] becomes [f x; g y]
let rec zipList fList xList =
  match fList, xList with
  | [], _
  | _, [] ->
    // either side empty, then done
    []
  | (f :: fTail), (x :: xTail) ->
    // new head + new tail
    (f x) :: (zipList fTail xTail)

let add10 x = x + 10
let add20 x = x + 20
let add30 x = x + 30

let result =
  let (<*>) = zipList
  [ add10; add20; add30 ] <*> [ 1; 2; 3 ]

let resultAdd =
  let (<*>) = zipList
  [ add; add ] <*> [ 1; 2 ] <*> [ 10; 20 ]
// resultAdd = [11; 22]
// [ (add 1 10); (add 2 20) ]
