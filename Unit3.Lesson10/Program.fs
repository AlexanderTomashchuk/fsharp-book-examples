open System
open Microsoft.FSharp.Core

type Address =
  { Street: string
    Town: string
    City: string }

type Address2 = { Line1: string; Line2: string }

type Customer =
  { Forename: string
    Surname: string
    Age: int
    Address: Address
    EmailAddress: string }

let customer =
  { Forename = "Alex"
    Surname = "Tom"
    Age = 30
    Address =
      { Street = "Peremohy"
        Town = "Town"
        City = "Kyiv" }
    EmailAddress = "at@gmail.com" }

let updatedCustomer =
  { customer with
      Age = 31
      Surname = "Tomas" }

let city = customer.Address.City

type Car =
  { Manufacturer: string
    EngineSize: float
    NumberOfDoors: int }

let car =
  { Manufacturer = "BMW"
    EngineSize = 6.0
    NumberOfDoors = 5 }

//explicitly declaring the type of the car value
let address2: Address =
  { Street = "StreetName"
    Town = "TownName"
    City = "CityName" }

let address3 =
  { Address.Street = "StreetName"
    Address.Town = "TownName"
    City = "CityName" }

let isSameAddress = address2 = address3
//or
let isSameAddress2 = (address2 = address3)

type Order =
  { OrderId: string
    Name: string
    Price: decimal }

let order1 =
  { OrderId = "1"
    Name = "Order1"
    Price = 200.0m }
let order2 =
  { OrderId = "1"
    Name = "Order1"
    Price = 200.0m }

let ordersAreEqual = order1 = order2
let ordersAreEqual2 = order1.Equals(order2)
let ordersAreEqual3 = Object.ReferenceEquals(order1, order2) 

//Create a function that takes in a customer and, using copy-and-update syntax,
//sets the customer’s Age to a random number between 18 and 45
//The function should then print the customer’s original and new age,
//returning the updated customer record.

let setRandomAge customer =
  let updatedCustomer = { customer with Age = (Random()).Next(18,45)}
  printfn "Customer age before updating: %d" customer.Age
  printfn "Customer age after updating: %d" updatedCustomer.Age
  updatedCustomer
  
let newCustomer = setRandomAge customer

let customerTest = {
  Forename = "Kate"
  Surname = "Bar" 
  Age = failwith "todo"
  Address = failwith "todo"
  EmailAddress = failwith "todo" }

let funcTest() =
  //shadowing
  let myHome = { Street = "The Street"; Town = "The Town"; City = "The City" }
  let myHome = { myHome with City = "The Other City" }
  let myHome = { myHome with City = "The Third City" }
  printfn "%A" myHome
  printfn "%O" myHome
  ()

funcTest()