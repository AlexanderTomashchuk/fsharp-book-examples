open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

for number in 1 .. 10 do
  printf "%d " number
//output: 1 2 3 4 5 6 7 8 9 10

for number in 10 .. -1 .. 1 do
  printf "%d " number
//output: 10 9 8 7 6 5 4 3 2 1

for number in 2 .. 2 .. 10 do
  printf "%d " number
//output: 2 4 6 8 10


open System.IO

let reader =
  new StreamReader(File.OpenRead "README.md")

while (not reader.EndOfStream) do
  printfn "%s" (reader.ReadLine())

//comprehensions - something like System.Linq.Enumerable.Range in C#
open System

let arrayOfChars =
  [| for c in 'a' .. 'z' -> Char.ToUpper c |]

let listOfSquares = [| for i in 1 .. 10 -> i * i |]

let seqOfStrings =
  seq { for i in 2 .. 4 .. 20 -> sprintf "Number %d" i }

seqOfStrings |> Seq.iter (fun s -> printfn "%s" s)

//pattern matching
let getCreditLimit customer =
  match customer with
  | "medium", 1 -> 500
  | "good", 0
  | "good", 1 -> 750
  | "good", 2 -> 1000
  | "good", _ -> 2000
  | _ -> 250

let getCreditLimit2 customer =
  match customer with
  | "medium", 1 -> 500
  | "good", years when years < 2 -> 750
  | "good", 2 -> 1000
  | "good", _ -> 2000
  | _ -> 250

let getCreditLimit3 customer =
  match customer with
  | "medium", 1 -> 500
  | "good", years ->
    match years with
    | 0
    | 1 -> 750
    | 2 -> 1000
    | _ -> 2000
  | _ -> 250

let customer = ("bad", 0)
let creditLimit = getCreditLimit customer

type Customer = { Name: string; Balance: decimal }

let handleCustomersBad (customers: Customer list) =
  if customers.IsEmpty then
    failwith "seq is empty"
  elif customers.Length = 1 then
    printfn "Customer name: %s" customers.[0].Name
  elif customers.Length = 2 then
    printfn "Customers balance is: %M" (customers |> List.sumBy (fun c -> c.Balance))
  else
    printfn "Total number of customers: %d" customers.Length

let handleCustomers customers =
  match customers with
  | [] -> failwith "seq is empty"
  | [ customer ] -> printfn "Single customer, name is %s" customer.Name
  | [ first; second ] -> printfn "Two customers, balance = %M" (first.Balance + second.Balance)
  | customers -> printfn "Customers supplied: %d" customers.Length

let customers1 = [] |> handleCustomers

let customers2 =
  [ { Name = "Alex"; Balance = 100M } ]
  |> handleCustomers

let customers3 =
  [ { Name = "Alex"; Balance = 100M }
    { Name = "Kate"; Balance = 1999M } ]
  |> handleCustomers

let customers4 =
  [ { Name = "Alex"; Balance = 100M }
    { Name = "Kate"; Balance = 1999M }
    { Name = "Valentin"; Balance = 20000M } ]
  |> handleCustomers

//pattern matching with records
let getStatus customer =
  match customer with
  | { Balance = 0M } -> "Customer has empty balance!"
  | { Name = "Alex" } -> "This is a great customer!"
  | { Name = name; Balance = 50M } -> sprintf "%s has a great balance!" name
  | { Name = name } -> sprintf "%s is a normal customer" name

{ Name = "Joe"; Balance = 50M } |> getStatus

type Customer2 = { Name: string; Balance: int }

let test customers =
  match customers with
  | [ { Name = "Tanya" }; { Balance = 25 }; _ ] -> "It's a match!"
  | _ -> "No match!" 
//  match customers with
//  | [ { Name = "Kate" }; { Balance = 25M }; _ ] -> "It's a match!"
//  | _ -> "No match!"
let lCustomers =
  [ { Name = "Alex"; Balance = 100M }
    { Name = "Kate"; Balance = 1999M }
    { Name = "Valentin"; Balance = 20000M } ]

lCustomers |> test
