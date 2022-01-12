let sum inputs =
  let mutable accumulator = 0

  for input in inputs do
    accumulator <- accumulator + input

  accumulator

let sumRecursion inputs =
  let rec sumAux acc list =
    match list with
    | [] -> acc
    | head :: tail -> sumAux (acc + head) tail

  sumAux 0 inputs

let length inputs =
  let mutable accumulator = 0

  for _ in inputs do
    accumulator <- accumulator + 1

  accumulator

let lengthRecursion inputs =
  let rec lengthAux acc list =
    match list with
    | [] -> acc
    | _ :: tail -> lengthAux (acc + 1) tail

  lengthAux 0 inputs

let max inputs =
  let mutable maxValue = System.Int32.MinValue

  for input in inputs do
    if maxValue < input then
      maxValue <- input
    else
      maxValue <- maxValue

  maxValue

let maxRecursion inputs =
  let rec maxAux maxVal list =
    match list with
    | [] -> maxVal
    | head :: tail ->
      match (maxVal, head) with
      | (maxVal, head) when maxVal < head -> maxAux head tail
      | (maxVal, head) when maxVal >= head -> maxAux maxVal tail
      | _ -> failwith "todo"


  maxAux System.Int32.MinValue inputs

let collection = [ 1; 2; 3; 4; 5; -1; 0; -3 ]

let collectionSum = collection |> sum
let collectionSumUsingRecursion = collection |> sumRecursion
let collectionLength = collection |> length
let collectionLengthUsingRecursion = collection |> lengthRecursion
let collectionMaxValue = collection |> max
let collectionMaxValueUsingRecursion = collection |> maxRecursion

//recursion
let rec factorial number total =
  if number = 1 then
    total
  else
    printfn "Number %d" number
    factorial (number - 1) (total * number)

let total = factorial 5 1

//fold
let sumFold inputs =
  Seq.fold
    (fun state input ->
      let newState = state + input
      printfn "Current state is %d, input is %d, new state value is %d" state input newState
      newState)
    0
    inputs

let lengthFold inputs =
  (0, inputs)
  ||> Seq.fold (fun state _ -> state + 1)

let maxFold inputs =
  inputs
  //Seq.reduce - the simplified version of Seq.fold, using the first element in the collection as the initial state
  |> Seq.reduce
       (fun state input ->
         match (state, input) with
         | state, input when state > input -> state
         | state, input when state <= input -> input
         | _ -> failwith "todo")

let newCollection = [ -3; -2; -1; 0; 100; 2; 3; 4; 0 ]
let newCollectionSum = newCollection |> sumFold
let newCollectionLength = newCollection |> lengthFold
let newCollectionMax = newCollection |> maxFold

//while loops example
open System.IO
let mutable totalChars = 0

let sr =
  new StreamReader(File.OpenRead "./test/book.txt")

while (not sr.EndOfStream) do
  let line = sr.ReadLine()
  totalChars <- totalChars + line.ToCharArray().Length

//folding instead of while loops
let lines: string seq =
  seq {
    use sr =
      new StreamReader(File.OpenRead "./test/book.txt")

    while (not sr.EndOfStream) do
      yield sr.ReadLine()
  }

let charsCountInFile =
  (0, lines)
  ||> Seq.fold (fun total line -> total + line.Length)

//composing functions with fold - rules engine example
open System
type Rule = string -> bool * string

let rules: Rule list =
  [ fun text -> (text.Split ' ').Length = 3, "Must be three words"
    fun text -> text.Length <= 30, "Max length is 30 characters"
    fun text ->
      text
      |> Seq.filter Char.IsLetter
      |> Seq.forall Char.IsUpper,
      "All letters must be caps"
    fun text -> text |> Seq.forall(fun n -> Char.IsNumber(n) = false), "Should not contain numbers" ]

let buildValidator (rules: Rule list) =
  rules
  |> List.reduce
       (fun firstRule secondRule ->
         fun word ->
           let passed, error = firstRule word

           if passed then
             let passed, error = secondRule word

             if passed then
               true, ""
             else
               false, error
           else
             false, error)

let validate = buildValidator rules
let word = "HELLO FROM F#3FFF"
let validationResult = word |> validate
