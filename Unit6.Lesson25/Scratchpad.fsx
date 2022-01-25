#r "../Unit6.Lesson25.CSharp/bin/Debug/net6.0/Unit6.Lesson25.CSharp.dll"

open System
open System.Collections.Generic
open Unit6.Lesson25.CSharp

let alex = Person "Alex"
alex.PrintName()

let longhand =
  [ "Tony"; "Fred"; "Alex"; "Kate" ]
  |> List.map (fun name -> Person(name))

let shorthand =
  [ "Tony"; "Fred"; "Alex"; "Kate" ]
  |> List.map Person

type PersonComparer() =
  interface IComparer<Person> with
    member this.Compare(x, y) = x.Name.CompareTo(y.Name)

let pComparer = PersonComparer() :> IComparer<Person>
pComparer.Compare(alex, Person "Fred")

//object expressions
let oPComparer =
  { new IComparer<Person> with
      member this.Compare(x, y) = x.Name.CompareTo(y.Name) }

//nulls, nullables and options
let blank:string = null
let name = "Vera"
let number = Nullable 10
let blankAsOption = blank |> Option.ofObj
let nameAsOption = name |> Option.ofObj
let numberAsOption = number |> Option.ofNullable
let unsafeName = Some "Fred" |> Option.toObj
