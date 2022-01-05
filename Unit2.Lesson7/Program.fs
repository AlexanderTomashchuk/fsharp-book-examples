open System
open System.IO
open System.Net

let describeAge age =
  let ageDescription =
    if age < 18 then "Child"
    elif age < 65 then "Adult"
    else "OAP"

  let greeting = "Hello"
  Console.WriteLine($"{greeting}! You are {ageDescription}")

describeAge 30
describeAge 10

let writeTextToDisk text =
  let path = Path.GetTempFileName()
  File.WriteAllText(path, text)
  path
  
let createManyFiles =
  ignore(writeTextToDisk "text1")
  ignore(writeTextToDisk "text2")
  writeTextToDisk "text3"

createManyFiles

//forcing statement-based evaluation
let now = DateTime.UtcNow.TimeOfDay.TotalHours

if now <12.0 then Console.WriteLine "It is morning"
elif now < 18.0 then Console.WriteLine "It is afternoon"
elif now < 20.0 then ignore(5+5)
else ()

//Then, create a program that can read the user’s full name from the console and print
//the user’s first name only. Thinking about expressions, can you write the application
//so that the main logic is expression- based? What impact does this have on coupling to the console?

let readUsername =
  let unvalidatedUsername = Console.ReadLine()
  let splitFnAndLn = unvalidatedUsername.Split(" ")
  (splitFnAndLn[0], splitFnAndLn[1]) 
   
let writeOnlyFirstName (firstName: string) =
  Console.WriteLine firstName

let (firstname, lastname) = readUsername
writeOnlyFirstName firstname