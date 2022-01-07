//tupled form of function

open System
open System.IO
open System.Reflection

let tupledAdd (a, b) = a + b
let answer = tupledAdd (5,10)

//curried form of function
let curriedAdd a b = a + b
let answer2 = curriedAdd 4 14

//calling a curried function in steps
let add first second = first + second
let addFive = add 5
//the same as the following
let testAdd 5 second = 5 + second
let fifteen = addFive 10

//explicitly created wrapper functions
let buildDt year month day = DateTime(year, month, day)
let buildDtThisYear month day = buildDt DateTime.UtcNow.Year month day
let buildDtThisMonth day = buildDtThisYear DateTime.UtcNow.Month day

//creating wrapper functions by currying
let buildDtThisYear2 = buildDt DateTime.UtcNow.Year
let buildDtThisMonth2 = buildDtThisYear2 DateTime.UtcNow.Month

let writeToFile (date:DateTime) (fileName:string) text =
  let path = $"""{date.ToString("yyMMdd")}-{fileName}.txt"""
  File.WriteAllText(path, text)

let writeToToday = writeToFile DateTime.UtcNow.Date
let writeToTomorrow = writeToFile (DateTime.UtcNow.Date.AddDays 1)
let writeToTodayHelloWorld = writeToToday "hello-world"

writeToToday "first-file" "test1"
writeToTomorrow "second-file" "test2"
writeToTodayHelloWorld "test3"

//calling functions arbitrary
let checkCreation (time:DateTime) =
  let msg =
    if time.Date.AddDays(7) > DateTime.UtcNow then "New"
    else "Old"
  msg
  
let time =
  let directory = Directory.GetCurrentDirectory()
  Directory.GetCreationTimeUtc directory

let folderDescr = checkCreation time
printfn $"Folder description: %s{folderDescr}"

//pipes
Directory.GetCurrentDirectory()
|> Directory.GetCreationTimeUtc
|> checkCreation
|> printfn "Folder description: %s"

let drive distance petrol =
  if distance > 50 then petrol / 2.0
  elif distance > 25 then petrol - 10.0
  elif distance > 0 then petrol - 1.0
  else petrol

let petrol = 100.0

let result =
  petrol
  |> drive 100
  |> drive 30
  |> drive 10
  |> drive 0
  |> drive 0

//functions composition
let checkCurrentDirectoryAge =
  Directory.GetCurrentDirectory
  >> Directory.GetCreationTimeUtc
  >> checkCreation

let description = checkCurrentDirectoryAge()

