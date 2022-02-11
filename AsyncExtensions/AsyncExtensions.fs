open System.Threading

module Async =
  let private combine f g x =
    async {
      let! v = f x
      return! g v
    }

  let (>=>!) = combine

  let ret x = async { return x }

  let map f x =
    async {
      let! v = x
      return f v
    }

  //https://stackoverflow.com/questions/30416774/when-calling-an-f-async-workflow-can-i-avoid-defining-a-temporary-label
  //leave a comment
  let bind f x =
    async {
      let! v = x
      return! f v
    }
  
  let (>>=!) x f = bind f x

open Async

let add1Async x = async { return x + 1 }
let doubleAsync x = async { return x * 2 }

let addAsync x y =
  async {
    Thread.Sleep 1000
    return x + y
  }

let multipleAsync x y =
  async {
    Thread.Sleep 2000
    return x * y
  }


//test of >>!
let add1ThenDoubleAsync = add1Async >=>! doubleAsync

add1ThenDoubleAsync 5
|> Async.RunSynchronously
|> printfn "%d"

let add5ThenMultiplyBy10Async = addAsync 5 >=>! multipleAsync 10

add5ThenMultiplyBy10Async 3
|> Async.RunSynchronously
|> printfn "%d"

//test of bind
let asyncResult1 =
  5 |> add1Async |> Async.bind doubleAsync

asyncResult1
|> Async.RunSynchronously
|> printfn "%d"

let asyncResult2 =
  5
  |> addAsync 4
  |> Async.bind (multipleAsync 10)

asyncResult2
|> Async.RunSynchronously
|> printfn "%d"

//test of bind using operator >>=!
open Async

let asyncResult3 = 5 |> add1Async >>=! doubleAsync

asyncResult3
|> Async.RunSynchronously
|> printfn "%d"

let asyncResult4 = 5 |> addAsync 4 >>=! multipleAsync 10

asyncResult4
|> Async.RunSynchronously
|> printfn "%d"
