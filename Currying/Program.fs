//normal version
let printTwoParameters x y = printfn "x=%d; y=%d" x y

//explicitly curried function
let printTwoParametersE x =
  let subFunction y = printfn "x=%d; y=%d" x y
  subFunction

//step by step version
let x = 5
let y = 10
let intermediateFn = printTwoParameters x
let result = intermediateFn y

//inline version of above
let result2 = (printTwoParameters x) y

//normal version
let result3 = printTwoParameters x y

//another example
let addTwoParameters x y = x + y

//explicitly curried function
let addTwoParametersE x =
  let subFunction y = x + y
  subFunction

//now use it step by step
let i = 5
let k = 100
let intermediateAddTwo = addTwoParameters i
let result4 = intermediateAddTwo k

//inline version of above
let result5 = (addTwoParameters i) k

//normal version
let result6 = addTwoParameters i k

//using plus as a single value function
let j = 4
let m = 3
let intermediatePlus = (+) j
let result7 = intermediatePlus m

//using plus as a function with two parameters
let result8 = (+) j m

//normal version of plus as infix operator
let result9 = j + m

// create a function
let printHello() = printfn "hello"

printHello

let addXY x y =
  printfn "x=%i y=%i" x
  x + y

addXY 5 1

let reader = new System.IO.StringReader("hello");

let line1 = reader.ReadLine        // wrong but compiler doesn't
                                   // complain
printfn "The line is %s" line1     //compiler error here!
// ==> error FS0001: This expression was expected to have
// type string but here has type unit -> string

let line2 = reader.ReadLine()      //correct
printfn "The line is %s" line2     //no compiler error

let add1 x = x + 1
let x = add1 2 3
// ==>   error FS0003: This value is not a function
//                     and cannot be applied

let add1 x = x + 1
let intermediateFn = add1 2   //returns a simple value
let x = intermediateFn 3      //intermediateFn is not a function!
// ==>   error FS0003: This value is not a function
//                     and cannot be applied