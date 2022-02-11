open System.Threading

module Async =
  let private combine f g x =
    async {
      let! v = f x
      return! g v
    }

  let (>>!) = combine

  //todo: test it
  let ret x = async { return x }

  //todo: test it
  let map f x =
    async {
      let! v = x
      return f v
    }

  let mapAndUnwrap f x =
    async {
      let! v = x
      return! f v
    }

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
let add1ThenDoubleAsync = add1Async >>! doubleAsync

add1ThenDoubleAsync 5
|> Async.RunSynchronously
|> printfn "%d"

let add5ThenMultiplyBy10Async = addAsync 5 >>! multipleAsync 10

add5ThenMultiplyBy10Async 3
|> Async.RunSynchronously
|> printfn "%d"

//test of mapAndUnwrap
let asyncResult1 =
  5 |> add1Async |> Async.mapAndUnwrap doubleAsync

asyncResult1
|> Async.RunSynchronously
|> printfn "%d"

let asyncResult2 =
  5
  |> addAsync 4
  |> Async.mapAndUnwrap (multipleAsync 10)

asyncResult2
|> Async.RunSynchronously
|> printfn "%d"
