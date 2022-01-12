//standard dictionary functionality in f# (they are MUTABLE)
open System
open System.Collections.Generic
open System.IO
open Microsoft.FSharp.Collections

let inventory = Dictionary()

inventory.Add("Apples", 0.33)
inventory.Add("Oranges", 0.23)
inventory.Add("Bananas", 0.45)

inventory.Remove "Oranges" |> ignore

let bananas = inventory.["Bananas"]
let oranges = inventory.["Oranges"]

//immutable IDictionary
let inventoryImmutable =
  [ "Apples", 0.33
    "Oranges", 0.23
    "Bananas", 0.45 ]
  |> dict

let bananasImmutable = inventoryImmutable.["Bananas"]
//inventoryImmutable.Add("Pineapples", 0.85)
//inventoryImmutable.Remove("Bananas")
bananasImmutable

//Map - immutable key/value lookup
let inventory3 =
  [ "Apples", 0.33
    "Oranges", 0.23
    "Bananas", 0.45 ]
  |> Map.ofList

let bananas3 = inventory.["Bananas"]
let oranges3 = inventory.["Oranges"]

let newInventory =
  inventory3
  |> Map.add "Pineapples" 0.83
  |> Map.remove "Apples"

let pineapples = newInventory |> Map.tryFind "Pineapples"

let cheapFruit, expensiveFruit =
  inventory3
  |> Map.partition (fun fruit cost -> cost < 0.4)

let toDictionary = Map.toSeq >> dict

let toMap dictionary =
  (dictionary :> seq<_>)
  |> Seq.map (|KeyValue|)
  |> Map.ofSeq

let getRootDirInfo =
  let rootDirectories =
    "~/"
    |> Directory.GetDirectoryRoot
    |> Directory.GetDirectories
    |> Seq.map DirectoryInfo
    |> Seq.map (fun di -> (di.Name, di.CreationTimeUtc))
    |> Map.ofSeq
    |> Map.map (fun dirName creationTime -> (dirName, (DateTime.UtcNow - creationTime).Days))
    |> toDictionary

  rootDirectories

let stringMap =
  getRootDirInfo
  |> toMap
  |> Map.iter (fun _ (key, value) -> printfn "%s - %d" key value)

//Sets - collections of distinct objects
let myBasket =
  [ "Apples"
    "Apples"
    "Apples"
    "Bananas"
    "Pineapples" ]

let fruitsILike = myBasket |> Set.ofList
//output: set ["Apples"; "Bananas"; "Pineapples"; "bananas"]

let yourBasket = [ "Kiwi"; "Bananas"; "Grapes" ]
let allFruitsList = myBasket @ yourBasket |> List.distinct
let fruitsYouLike = yourBasket |> Set.ofList
let allFruits = fruitsILike + fruitsYouLike

let fruitsJustForMe = allFruits - fruitsYouLike

let fruitsWeCanShare =
  fruitsILike |> Set.intersect fruitsYouLike

let doILikeAllYourFruits =
  fruitsILike |> Set.isSubset fruitsYouLike


//HOMEWORK
//Continuing from the previous lesson, create a lookup for all files within a folder so that you
//can find the details of any file that has been read. Experiment with sets by identifying file
//types in folders. What file types are shared between two arbitrary folders?
type DirInfo =
  { Name: string
    Size: int64
    NumberOfFiles: int
    AverageFileSize: float
    SetOfFileExtensions: Set<string>
    Files: Map<string, FileInfo> }

let getDirInfo path =
  let dirInfo =
    Directory.GetDirectories(path)
    |> Array.map
         (fun iDir ->
           (iDir,
            iDir
            |> Directory.GetFiles
            |> List.ofArray
            |> List.map FileInfo))
    |> Array.map
         (fun (iDir, fileInfos) ->
           { Name = iDir
             Size = fileInfos |> List.sumBy (fun f -> f.Length)
             NumberOfFiles = fileInfos.Length
             AverageFileSize =
               float (fileInfos |> List.sumBy (fun f -> f.Length))
               / float fileInfos.Length
             SetOfFileExtensions =
               fileInfos
               |> List.map (fun fi -> fi.Extension)
               |> Set.ofList
             Files =
               fileInfos
               |> List.map (fun fi -> (fi.Name, fi))
               |> Map.ofList })

  dirInfo

getDirInfo "test"

let getSharedFileTypesBetweenDirs dirInfos =
  let sharedFileTypes =
      dirInfos
      |> Array.map(fun di -> di.SetOfFileExtensions)
      |> Seq.ofArray
      |> Set.intersectMany
  sharedFileTypes

let test = "test" |> getDirInfo |> getSharedFileTypesBetweenDirs
