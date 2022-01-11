//MAP

open System
open System.IO
open Microsoft.FSharp.Core

let numbers = [ 1 .. 10 ]
let timesTwo n = n * 2

//imperative style
let outputImperative = ResizeArray()

for number in numbers do
  outputImperative.Add(number |> timesTwo)

//declarative style
let outputDeclarative = numbers |> List.map timesTwo

[ "Isaac", 30
  "John", 25
  "Sarah", 18
  "Faye", 27 ]
|> List.map (fun (name, age) -> ())

//MAP2
let numbers2 = [ 11 .. 20 ]

let output =
  (numbers, numbers2)
  ||> List.map2 (fun x y -> x + y)

//ITER
numbers |> List.iter (fun i -> printfn "%d" i)

//COLLECT the same as SelectMany
type Order = { OrderId: int }

type Customer =
  { CustomerId: int
    Orders: Order list
    Town: string }

let customers =
  [ { CustomerId = 1
      Town = "Kyiv"
      Orders = [ { OrderId = 1 }; { OrderId = 2 } ] }
    { CustomerId = 2
      Town = "Lviv"
      Orders = [ { OrderId = 3 }; { OrderId = 4 } ] }
    { CustomerId = 3
      Town = "Odessa"
      Orders = [ { OrderId = 5 }; { OrderId = 6 } ] } ]

let collectedOrders =
  customers
  |> List.filter (fun c -> c.CustomerId % 2 = 0)
  |> List.collect (fun c -> c.Orders)

//PAIRWISE
let pairwise =
  [ DateTime(2010, 5, 1)
    DateTime(2010, 6, 1)
    DateTime(2010, 6, 12)
    DateTime(2010, 7, 3) ]
  |> List.pairwise
  |> List.map (fun (a, b) -> b - a)
  |> List.map (fun time -> time.TotalDays)

//GROUPBY
type Order2 =
  { Id: int
    Name: string
    Price: decimal }

let orders2 =
  [ { Id = 1; Name = "1"; Price = 1M }
    { Id = 1; Name = "1"; Price = 1M }
    { Id = 3; Name = "3"; Price = 3M } ]

let sameOrders =
  orders2 |> List.groupBy (fun o -> o.Name)

//COUNTBY
let countOfSameOrders = orders2 |> List.countBy (fun o -> o.Id)

//PARTITION
type Customer2 = { Name: string; Town: string }

let customers2 =
  [ { Name = "Isaac"; Town = "London" }
    { Name = "Sara"; Town = "Kyiv" }
    { Name = "Alex"; Town = "London" }
    { Name = "Tom"; Town = "Odessa" } ]

let londonCustomers, otherCustomers =
  customers2
  |> List.partition (fun c -> c.Town = "London")

//CHUNKBYSIZE
[ 1 .. 10 ] |> List.chunkBySize 3
//output: [[1; 2; 3]; [4; 5; 6]; [7; 8; 9]; [10]]

//SPLITINTO
[ 1 .. 10 ] |> List.splitInto 2
//output: [[1; 2; 3; 4; 5]; [6; 7; 8; 9; 10]]

//SPLITAT Splits a list into two lists, at the given index.
let input = [ 8; 4; 3; 1; 6; 1 ]
let front, back = input |> List.splitAt 5
//output: front = [8; 4; 3; 1; 6], back = [1]

//Aggregate functions
let numbers3 = [ 1.0 .. 10.0 ]
let total = numbers3 |> List.sum
let average = numbers3 |> List.average
let max = numbers3 |> List.max
let min = numbers3 |> List.min

//converting between collections
let numberOne =
  [ 1 .. 5 ]
  |> List.toArray
  |> Seq.ofArray
  |> Seq.head


//HOMEWORK
//Write a simple script that, given a folder path on the local filesystem, will return the
//name and size of each subfolder within it. Use groupBy to group files by folder, before
//using an aggregation function such as sumBy to total the size of files in each folder.
//Then, sort the results by descending size. Enhance the script to return a proper F# record
//that contains the folder name, size, number of files, average file size, and the distinct
//set of file extensions within the folder.
type DirInfo =
  { Name: string
    Size: int64
    NumberOfFiles: int
    AverageFileSize: float
    SetOfFileExtensions: string list }

let getDirInfo path =
  let dirInfo =
    Directory.GetDirectories(path)
    |> List.ofArray
    |> List.map
         (fun iDir ->
           (iDir,
            iDir
            |> Directory.GetFiles
            |> List.ofArray
            |> List.map FileInfo))
    |> List.map
         (fun (iDir, fileInfos) ->
           { Name = iDir
             Size = fileInfos |> List.sumBy (fun f -> f.Length)
             NumberOfFiles = fileInfos.Length
             AverageFileSize = float (fileInfos |> List.sumBy(fun f -> f.Length)) / float fileInfos.Length 
             SetOfFileExtensions = fileInfos |> List.map(fun f-> f.Extension)|>List.distinct })

  dirInfo

getDirInfo "test"
