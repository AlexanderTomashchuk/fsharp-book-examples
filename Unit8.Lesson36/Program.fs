open System.Net
open System.Net.Http
open System.Threading
open FSharp.Data

printfn "Loading data!"
Thread.Sleep(5000)
printfn "Loading data!"
printfn "My name is Alex."

async {
  printfn "Loading data!"
  Thread.Sleep(5000)
  printfn "Loading data!"
}
|> Async.Start

printfn "My name is Alex."

let asyncHello: Async<string> = async { return "Hello" }
//compile error
//let length = asyncHello.Length
let text = asyncHello |> Async.RunSynchronously
let length = text.Length

let printThread text =
  printfn "Thread %d: %s" Thread.CurrentThread.ManagedThreadId text

let doWork () =
  printThread "Start loading running work!"
  Thread.Sleep 5000
  "HELLO"

let asyncLength: Async<int> =
  printThread "Creating async block"

  let asyncBlock =
    async {
      printThread "In block!"
      let text = doWork ()
      return (text + " WORLD").Length
    }

  printThread "Created async block"
  asyncBlock

let length2 = asyncLength |> Async.RunSynchronously

let getTextAsync = async { return "hello" }

let printHelloWorld =
  async {
    let! text = getTextAsync
    printfn "%s world" text
  }

printHelloWorld |> Async.Start

//fork/join
let random = System.Random()
let pickANumberAsync = async { return random.Next(10) }

let createFiftyNumbers =
  let workflows = [ for i in 1 .. 50 -> pickANumberAsync ]

  async {
    let! numbers = workflows |> Async.Parallel
    printfn "Total is %d" (numbers |> Seq.sum)
  }

createFiftyNumbers |> Async.Start


let downloadData (uri: string) =
  async {
    use httpClient = new HttpClient()
    let! response = httpClient.GetAsync(uri) |> Async.AwaitTask

    let! content =
      response.Content.ReadAsStringAsync()
      |> Async.AwaitTask

    return content.Length
  }

let urls =
  [ "http://www.fsharp.org"
    "http://microsort.com"
    "http://fsharpforfunandprofit.com" ]

let downloadedBytes =
  urls
  |> List.map (downloadData)
  |> Async.Parallel
  |> Async.RunSynchronously
  |> Seq.sum

//HOMEWORK
//Task 1
//Write an application to demonstrate the differences in terms of performance and
//threads between synchronous, multithreaded, and asynchronous parallel downloading
//of 10 HTTP resources.

//Task 2
//Then, try using the Async methods included in FSharp.Data for downloading
//JSON data from a remote resource.

//Task 3
//Finally, try to handle an exception raised in an async block by using the Async.Catch
//method.

//Task 1
open FSharp.Data

let httpUrls =
  [ "http://amazon.com"
    "http://google.com"
    "http://microsoft.com"
    "http://fsharp.org"
    "http://censor.net"
    "http://minfin.com.ua"
    "http://rozetka.com.ua"
    "http://linkedin.com"
    "http://twitter.com"
    "http://fsharpforfunandprofit.com"
    "http://amazon.com"
    "http://google.com"
    "http://microsoft.com"
    "http://fsharp.org"
    "http://censor.net"
    "http://minfin.com.ua"
    "http://rozetka.com.ua"
    "http://linkedin.com"
    "http://twitter.com"
    "http://fsharpforfunandprofit.com"
    "http://amazon.com"
    "http://google.com"
    "http://microsoft.com"
    "http://fsharp.org"
    "http://censor.net"
    "http://minfin.com.ua"
    "http://rozetka.com.ua"
    "http://linkedin.com"
    "http://twitter.com"
    "http://fsharpforfunandprofit.com" ]

//synchronous run
let downloadSiteSync (url: string) =
  let data = Http.RequestString(url)
  data.Length

#time

let bulkDownloadSync =
  httpUrls |> Seq.map (downloadSiteSync) |> Seq.sum //val bulkDownloadSync: int = 5104004

#time //Real: 00:01:02.883, CPU: 00:00:00.047, GC gen0: 10, gen1: 3, gen2: 0

//asynchronous run

let downloadSiteAsync url =
  async {
    let! data = url |> Http.AsyncRequestString
    return data.Length
  }

#time

let bulkDownloadAsync =
  httpUrls
  |> Seq.map downloadSiteAsync
  |> Async.Parallel
  |> Async.RunSynchronously
  |> Seq.sum //val bulkDownloadAsync: int = 5073901

#time //Real: 00:00:02.774, CPU: 00:00:00.013, GC gen0: 8, gen1: 3, gen2: 1

//multithreaded

#time

let bulkDownloadMultithreaded =
  httpUrls
  |> Seq.map downloadSiteAsync
  |> Async.Sequential
  |> Async.RunSynchronously
  |> Seq.sum //val bulkDownloadMultithreaded: int = 5096323

#time //Real: 00:00:22.632, CPU: 00:00:00.027, GC gen0: 10, gen1: 0, gen2: 0

//Task 2
let asyncGet url = Http.AsyncRequest url

let data =
  "https://gorest.co.in/public/v2/posts"
  |> asyncGet
  |> Async.RunSynchronously

//Task 3
let asyncGet url =
  async {
    let! httpResponseChoiceAsync = Http.AsyncRequest url |> Async.Catch

    return
      match httpResponseChoiceAsync with
      | Choice1Of2 httpResponse -> Some httpResponse
      | Choice2Of2 exp ->
        printfn "Error occured. Error message: %s" exp.Message
        None
  }

let data =
  "https://fdfdgorest.co.in/fdpublic/v2/posts"
  |> asyncGet
  |> Async.RunSynchronously
