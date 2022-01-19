open System
open System.Collections.Generic
open System.IO
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Core

let aNumber: int = 10
let maybeANumber: int option = Some 10

type Customer =
  { Name: string
    YearPassed: DateOnly
    SafelyScore: int option }

let customers =
  [ { Name = "Alex"
      YearPassed = DateOnly(1999, 1, 1)
      SafelyScore = None }
    { Name = "Kate"
      YearPassed = DateOnly(2010, 1, 1)
      SafelyScore = Some -100 } ]

let calculateAnnualPremiumUsd customer =
  match customer.SafelyScore with
  | Some 0 -> 250
  | Some score when score < 0 -> 400
  | Some score when score > 0 -> 150
  | None ->
    printfn "No score supplied! using temporary premium."
    300

let calculatedAnnualPremiums =
  customers |> List.map (calculateAnnualPremiumUsd)

let customer = customers.[1]

let describe safetyScore =
  if safetyScore > 200 then
    "Safe"
  else
    "High Risk"

//map
let description =
  match customer.SafelyScore with
  | Some score -> Some(describe score)
  | None -> None

let descriptionTwo =
  customer.SafelyScore
  |> Option.map (fun score -> describe score)

let shortHand =
  customer.SafelyScore |> Option.map describe

let optionalDescribe = Option.map describe

//iter
let printCustomerScore customer =
  customer.SafelyScore
  |> Option.iter (fun c -> c |> describe |> printfn "%s")

printCustomerScore customers.[0]

//bind
let tryFindCustomer cId =
  if cId = 10 then
    Some customers.[1]
  else
    None

let getSafetyScore customer = customer.SafelyScore

let score =
  tryFindCustomer 10 |> Option.bind getSafetyScore

//filter
let test1 = Some 5 |> Option.filter (fun x -> x > 5) //None
let test2 = Some 5 |> Option.filter (fun x -> x = 5) //Some 5

//count
let test3 =
  Some 5
  |> Option.filter (fun x -> x > 5)
  |> Option.count //0

let test4 =
  Some 5
  |> Option.filter (fun x -> x = 5)
  |> Option.count //1

//count
let test5 = Some 5 |> Option.exists (fun x -> x > 5) //false
let test6 = Some 5 |> Option.exists (fun x -> x = 5) //true

//toList
let test7 = Some 5 |> Option.toList //[5]

let test8 =
  Some 5
  |> Option.filter (fun x -> x > 10)
  |> Option.toList //[]

//List.choose
let list = [ 5; 7; 100; -12; 11 ]

let test9 =
  list
  |> List.choose (fun x -> if x > 5 then Some x else None)

let listWords = [ "and"; "Rome"; "Bob"; "apple"; "zebra" ]
let isCapitalized (string1:string) = Char.IsUpper string1[0]
let results = listWords |> List.choose(fun elem ->
  match elem with
  | elem when isCapitalized elem -> Some(elem + "'s")
  | _ -> None)
printfn "%A" results
results |> List.iter(printfn "%s")

type Customer2 = string option

let tryLoadCustomer id =
  match id with
  | id when id >= 2 && id <= 7 -> Some (sprintf "Customer #%d" id)
  | _ -> None
let customerIdsList = [1..10]
let successfullyLoadedCustomers = customerIdsList |> List.choose tryLoadCustomer

let ids = [1..100]
let findId id = ids |> List.tryFind(fun i -> i = id)
findId 101
findId 50

//HOMEWORK
//Write an application that displays information on a file on the local hard disk. If the file isn’t found,
//return None. Have the caller code handle both scenarios and print an appropriate response to the console.
open System.IO

let tryGetFileInfo path =
  let fileInfo = FileInfo(path)
  match fileInfo.Exists with
  | true -> Some(fileInfo.Name, fileInfo.Extension, fileInfo.Length)
  | false -> None

tryGetFileInfo "./test/book.txt"
tryGetFileInfo "./test/xyz.txt"
