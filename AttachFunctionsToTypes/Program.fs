//https://fsharpforfunandprofit.com/posts/type-extensions/

module Person =
  type T =
    { First: string
      Last: string }
    //member defined with type declaration
    member self.FullName = self.First + " " + self.Last
    static member Create first last = { First = first; Last = last }

  //constructor
  let create first last = { First = first; Last = last }

  //standalone function
  let fullName2 {First=first; Last=last} = first + " " + last
  
  //another method added later
  type T with
    member self.SortableName = self.Last + ", " + self.First
    member self.FullName2 = fullName2 self

//test
let person = Person.create "Alex" "Tom"
let printFullName = person.FullName |> printfn "%s"
let printSortableName = person.SortableName |> printfn "%s"

//in a different module
module PersonExtensions =
  type Person.T with
    member self.UppercaseName = self.FullName.ToUpper()

//test
open PersonExtensions
let printUppercaseFullName = person.UppercaseName |> printfn "%s"

//extending system types
type System.Int32 with
  member self.isEven = self % 2 = 0
  static member IsOdd x = x % 2 = 1

//test
let i = 2
i.isEven |> printfn "%d is even %b" i

//test of static method
let person2 = Person.T.Create "Tom" "Jones"
person2.FullName |> printfn "%s"

type System.Double with
  static member PI = 3.141

//test
123 |> System.Int32.IsOdd |> printfn "Is odd %b"
124 |> System.Int32.IsOdd |> printfn "Is odd %b"

System.Double.PI |> printfn "PI number is %f"

//attaching existing functions
let list = [ 0 .. 10 ]

//functional style
let len1 = list |> List.length

//OO style
let len2 = list.Length

module Order =
  //type with no members initially
  type V = { OrderId: string; Name: string }

  //constructor
  let create orderId name = { OrderId = orderId; Name = name }

  //standalone function
  let printOrderInfo { OrderId = orderId; Name = name } =
    printfn "OrderId: %s; Name: %s" orderId name

  let hasSameName (order: V) otherName = order.Name = otherName

  //attach preexisting function as a member
  type V with
    member this.print = printOrderInfo this
    member this.hasSameName = hasSameName this

//test
let order = Order.create "1" "Table"
order |> Order.printOrderInfo
order.print

//functional style
(order, "Table")
||> Order.hasSameName
|> printfn "%b"

(order, "Chair")
||> Order.hasSameName
|> printfn "%b"

//OO style
order.hasSameName "Table" |> printfn "%b"
order.hasSameName "Chair" |> printfn "%b"

type Product =
  { SKU: string
    Price: float }
  //curried style
  member this.CurriedTotal qty discount = (this.Price * qty) - discount

  //tuple style
  member this.TupleTotal(qty, discount) = (this.Price * qty) - discount

  member this.TupleTotal2(qty, ?discount) =
    let extPrice = this.Price * qty

    match discount with
    | Some discount -> extPrice - discount
    | None -> extPrice

  member this.TupleTotal3(qty, ?discount) =
    (this.Price * qty)
    - (discount |> Option.defaultValue 0.0)

  member this.TupleTotal4(qty, ?discount) =
    let extPrice = this.Price * qty
    let discount = (discount, 0.0) ||> defaultArg
    extPrice - discount
  
  member this.TupleTotalOverloading(qty) =
    printfn "using non-discount method"
    this.Price * qty
    
  member this.TupleTotalOverloading(qty, discount) =
    printfn "using discount method"
    (this.Price * qty) - discount

//test
let product = { SKU = "ABS"; Price = 199.50 }

product.CurriedTotal 10 100
|> printfn "Total price: %f"

product.TupleTotal(10, 100)
|> printfn "Total price: %f"

let totalFor10 = product.CurriedTotal 10
let discounts = [ 1.0 .. 5.0 ]
let totalForDifferentDiscounts = discounts |> List.map totalFor10

//named parameters with tuple-style parameters
let product2 = { SKU = "BDF"; Price = 2.0 }

let total2 =
  product2.TupleTotal(qty = 10, discount = 5.0)

let total3 =
  product2.TupleTotal(discount = 5.0, qty = 10)

//optional parameters with tuple-style parameters
let product3 = { SKU = "GEG"; Price = 3.0 }
let total4 = product3.TupleTotal3(10)
let total5 = product3.TupleTotal3(10, 2.0)

//method overloading
//F# does support method overloading, but only for methods
//(that is functions attached to types) and of these, only those using tuple-style parameter passing.
let product4 = {SKU ="SSS"; Price = 5.0}
let total6 = product4.TupleTotalOverloading(10)
let total7 = product4.TupleTotalOverloading(10, 5.0)

//Hey! Not so fast… The DOWNSIDES of using methods
//1. Methods don’t play well with type inference
open Person

//using standalone function
let printFullName2 person =
  printfn "Name is: %s" (fullName2 person)

//OK
printFullName2 person2


//using method with "dotting into" (ISSUE 1)
let printFullName3 person =
  printfn "Name is %s" (person.FullName2)

//2. Methods don't play well with high order functions

//using standalone function
let list = [
  Person.create "Andy" "Anderson";
  Person.create "John" "Johnson";
  Person.create "Jack" "Jackson"]

list |> List.map fullName2

//with object methods, we have to create special lambdas everywhere
list |> List.map(fun p -> p.FullName2)