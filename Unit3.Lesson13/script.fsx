open System
open System.IO
open System.Net
open System.Net.Http

type Customer = { Age: int }

let printCustomerAge writer customer =
  let ageDesc =
    if customer.Age < 13 then "Child!"
    elif customer.Age < 20 then "Teenager!"
    else "Adult!"

  writer $"Customer is %s{ageDesc}"

let customer =
  {

    Age = 30 }

printCustomerAge Console.WriteLine customer

//partially applied function
let printToConsole = printCustomerAge Console.WriteLine

printToConsole { Age = 10 }
printToConsole { Age = 30 }

let writeToFile text =
  let path = Path.GetTempFileName()
  printfn "%s" path
  File.WriteAllText(path, text)

let printToFile = printCustomerAge writeToFile

printToFile { Age = 17 }

//Create a set of functions that use another dependency in .NET—for example,
//working with HTTP data by using WebClient. Write a function that takes in
//the HTTP client to GET data to a URI. What’s the dependency? The WebClient
//class, or a function on the WebClient?

let sendRequest sender (uri:string) =
  let msg = new HttpRequestMessage(HttpMethod.Get, uri)
  let result = sender msg
  result
  
let sendHttpRequest = sendRequest (new HttpClient()).Send

sendHttpRequest"https://google.com"