let add (a: int, b: int) : int =
  let answer: int = a + b
  answer

let add2 (a: int, b: int) =
  let answer: int = a + b
  answer

let add3 (a: int, b) =
  let answer: int = a + b
  answer
  
let add4 (a, b) =
  let answer: int = a + b
  answer

let add5 (a, b) =
  let answer = a + b
  answer

//doesn't compile
//let getLength name = sprintf "Name is %d letters." name.Length
let getLength (name: string) = sprintf "Name is %d letters." name.Length
let foo name = "Hello! " + getLength(name)

open System.Collections.Generic
let numbers = List<_>()
numbers.Add(10)
numbers.Add(20)

let otherNumbers = List()
otherNumbers.Add(10) 
otherNumbers.Add(20)

let createList(first, second) =
  let output = List()
  output.Add(first)
  output.Add(second)
  output

let sayHello(someValue) =
  let innerFunction(number) =
    if number > 10 then "Isaac"
    elif number >  20 then "Fred"
    else "Sara"
  let resultOfInner=
    if someValue < 10.0 then innerFunction(5)
    else innerFunction(15)
  "hello " + resultOfInner
let result = sayHello(10.5)