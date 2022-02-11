
//https://fsharpforfunandprofit.com/posts/elevated-world-2/

//the bind function
//Common Names: bind, flatMap, andThen, collect, SelectMany
//Common Operators: >>= (left to right), =<< (right to left )
//What it does: Allows you to compose world-crossing (“monadic”) functions
//Signature: (a->E<b>) -> E<a> -> E<b>. Alternatively with the parameters reversed: E<a> -> (a->E<b>) -> E<b>

module Option =
  let bindOption f xOpt =
    match xOpt with
    | Some v -> f v
    | None -> None

  let (>>=) xOpt f = bindOption f xOpt

  let mapOption f = bindOption (f >> Some)

  let apply fOpt xOpt =
    fOpt
    |> Option.bind
         (fun f ->
           let map = Option.bind (f >> Some)
           map xOpt)

module List =
  //the same as List.collect
  let bindList (f: 'a -> 'b list) (xList: 'a list) =
    [ for x in xList do
        for y in f x do
          yield y ]

  let (>>=) = bindList

let parseInt str =
  match str with
  | "-1" -> Some 1
  | "0" -> Some 0
  | "1" -> Some 1
  | "2" -> Some 2
  | "3" -> Some 3
  //etc
  | _ -> None

type OrderQty = OrderQty of int

let toOrderQty qty =
  if qty >= 1 then
    Some(OrderQty qty)
  else
    // only positive numbers allowed
    None

"0"
|> parseInt
|> Option.bindOption toOrderQty
|> printfn "%A" //None

"3"
|> parseInt
|> Option.bindOption toOrderQty
|> printfn "%A" //Some(OrderQty 3)

"10"
|> parseInt
|> Option.bindOption toOrderQty
|> printfn "%A" //None

//or
open Option

"2" |> parseInt >>= toOrderQty |> printfn "%A"
"10" |> parseInt >>= toOrderQty |> printfn "%A"
