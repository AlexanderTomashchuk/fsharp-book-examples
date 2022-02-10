open System.Collections.Generic

let F x y z = x y z

let f (x: int) = float x * 3.0
let g (x: float) = x > 4

let compose1 x = g (f x)
let compose2 f g x = g (f x)

let result1 = compose1 5
let result2 = compose2 f g 2

//operators
let (||>>) f g x = g (f x)

let compose3 = f ||>> g

let result3 = compose3 4

let add1 x = x + 1
let times2 x = x * 2
let add1Times2 = add1 ||>> times2

let result4 = add1Times2 5

let add n x = x + n
let times n x = x * n
let add1Times10 = add 1 >> times 10
let add5Times10 = add 5 >> times 10

let result5 = add1Times10 3
let result6 = add5Times10 3

let twice f = f >> f
let twiceAdd1 = twice add1

let result7 = twice add1 5

let add1ThenMultiply = (+) 1 >> (*)

let result8 = add1ThenMultiply 5 10

let times2add1 = add 1 << times 2

let result9 = times2add1 5

let myList = [ 1 ]
myList |> List.isEmpty |> not //straight pipeline
myList |> (not << List.isEmpty) //using reverse composition

let (||||>) x f = f x

let result10 = myList ||||> List.isEmpty ||||> not

let doSomething x y z = x+y+z
doSomething 1 2 3
3 ||||> doSomething 1 2