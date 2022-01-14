open System
open Microsoft.FSharp.Core
open Operations

let depositWithAudit =
  deposit
  |> auditAs "deposit" Auditing.composedTransaction

let withdrawWithAudit =
  withdraw
  |> auditAs "withdraw" Auditing.composedTransaction

let loadAccountFromDisk =
  FileRepository.findTransactionsOnDisk
  >> Operations.loadAccount

[<AutoOpen>]
module CommandParsing = 
  let isValidCommand command =
    [ "d"; "w"; "x" ] |> Seq.contains command
  let isStopCommand command = command = "x"

[<AutoOpen>]
module UserInput = 
  let commands =
    seq {
      while true do
        printfn "(d)eposit/(w)ithdraw/e(x)it:"
        yield Console.ReadLine()
        printfn ""
    }
    
  let getAmount command =
    printfn "Enter the amount:"
    command, Console.ReadLine() |> Decimal.Parse

[<EntryPoint>]
let main _ =
  let openingAccount =
    printf "Please enter your name: "
    Console.ReadLine() |> loadAccountFromDisk
    // or |> FileRepository.findTransactionsOnDisk |> loadAccount

  Auditing.printCurrentBalance openingAccount

  let processCommand account (command, amount) =
    let account =
      match command with
      | "d" -> account |> depositWithAudit amount
      | "w" -> account |> withdrawWithAudit amount
      | _ -> failwith "Invalid command"
    Auditing.printCurrentBalance account
    account

  let closingAccount =
    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount

  printfn $"Closing Balance:\r\n %A{closingAccount}"
  printfn "Enter any key to exit..."
  Console.ReadKey() |> ignore

  0
