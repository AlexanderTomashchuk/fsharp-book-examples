//https://fsharpforfunandprofit.com/posts/concurrency-async-and-parallel/

open System
open System.Threading
open System.Timers

let userTimerWithCallback =
  let event = new AutoResetEvent(false)
  let timer = new Timer(2000)

  timer.Elapsed.Add(fun _ -> event.Set() |> ignore)

  printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
  timer.Start()

  // keep working
  printfn "Doing something useful while waiting for event"

  // block on the timer via the AutoResetEvent
  event.WaitOne() |> ignore

  //done
  printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

let userTimerWithAsync =
  let timer = new Timer(2000)

  let timerEvent =
    Async.AwaitEvent timer.Elapsed |> Async.Ignore

  // start
  printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
  timer.Start()

  // keep working
  printfn "Doing something useful while waiting for event"

  // block on the timer event now by waiting for the async to complete
  Async.RunSynchronously timerEvent

  // done
  printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

let sleepWorkflow =
  async {
    printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay
    do! Async.Sleep 2000
    printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay
  }

sleepWorkflow |> Async.RunSynchronously

let nestedWorkflow =
  async {
    printfn "Starting parent"

    let! childWorkflow = Async.StartChild sleepWorkflow

    // give the child a chance and then keep working
    do! Async.Sleep 500
    printfn "Doing something useful while waiting"

    //block on the child
    let! result = childWorkflow

    //done
    printfn "Finished parent"
  }

nestedWorkflow |> Async.RunSynchronously

let testLoop =
  async {
    for i in [ 0 .. 100 ] do
      printf "%i before..." i
      do! Async.Sleep 10
      printfn ".. after"
  }

testLoop |> Async.RunSynchronously

//create a cancellation source
let cts = new CancellationTokenSource()

Async.Start(testLoop, cts.Token)

Thread.Sleep 200

cts.Cancel()

//Composing workflows in series and parallel

let sleepWorkflowMs ms =
  async {
    printfn "%i ms workflow started" ms
    do! Async.Sleep ms
    printfn "%i ms workflow finished" ms
  }

let workflowInSeries =
  async {
    let! sleep1 = sleepWorkflowMs 1000
    printfn "Finished one"
    let! sleep2 = sleepWorkflowMs 2000
    printfn "Finished second"
  }

#time
workflowInSeries |> Async.RunSynchronously
#time

let sleep1 = sleepWorkflowMs 1000
let sleep2 = sleepWorkflowMs 2000

//well known fork/join approach
#time

[ sleep1; sleep2 ]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

#time

open System.Net

let fetchUrl url =
  let req = WebRequest.Create(Uri(url))
  use resp = req.GetResponse()
  use stream = resp.GetResponseStream()
  use reader = new IO.StreamReader(stream)
  let html = reader.ReadToEnd()
  printfn "finished downloading %s" url

// a list of sites to fetch
let sites =
  [ "http://www.bing.com"
    "http://www.google.com"
    "http://www.microsoft.com"
    "http://www.amazon.com"
    "http://www.yahoo.com" ]

#time
sites |> List.map fetchUrl |> ignore
#time

let fetchUrlAsync url =
  async {
    let req = WebRequest.Create(Uri(url))
    use! resp = req.AsyncGetResponse() // new keyword "use!"
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    let html = reader.ReadToEnd()
    printfn "finished downloading %s" url
  }

#time

sites
|> List.map fetchUrlAsync
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

#time

let childTask () =
  // chew up some CPU.
  for i in [ 1 .. 10000 ] do
    for i in [ 1 .. 1000 ] do
      do "Hello".Contains("H") |> ignore
// we don't care about the answer!

#time
childTask ()
#time

let parentTask =
  childTask |> List.replicate 20 |> List.reduce (>>)

#time
parentTask ()
#time

let asyncChildTask = async { return childTask() }

let asyncParallelTask =
  asyncChildTask
  |> List.replicate(20)
  |> Async.Parallel

#time
asyncParallelTask |> Async.RunSynchronously |> ignore
#time
