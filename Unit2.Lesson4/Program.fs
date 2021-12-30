open System

let age = 35
let website = System.Uri "http://fsharp.org"
let add (first, second) = first + second

let count = 10
let random = Random 100
let randomNumber = random.Next(1, 100)

let foo() =
  let x = 10
  printfn "%d" (x+20)
  let x = "test"
  let x = 50.0
  x + 200.0

let unitFunc = foo()

let doStuffWithTwoNumbers(first, second) =
  let added = first + second
  printfn $"%d{first} + %d{second} = %d{added}"
  let doubled = added * 2
  doubled

let estimateAge =
  let age =
    let year = DateTime.Now.Year
    year - 1979
  $"You are about %d{age} years old!"

let estimateAges(familyName, year1, year2, year3) =
  let calculateAge yearOfBirth =
    let year = DateTime.Now.Year
    year - yearOfBirth
  
  let estimatedAge1 = calculateAge year1
  let estimatedAge2 = calculateAge year2
  let estimatedAge3 = calculateAge year3
  
  let averageAge = (estimatedAge1 + estimatedAge2 + estimatedAge3) / 3
  $"Average age for family %s{familyName} is %d{averageAge}"

let r = Random()
let nextValue = r.Next(1, 6)
let answer = nextValue + 10

let generateRandomNumber max =
  let r = Random()
  let nextValue = r.Next(1, max)
  nextValue

let generatedNumber = generateRandomNumber 100
printfn "%d" generatedNumber 