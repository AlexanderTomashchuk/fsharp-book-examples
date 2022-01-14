#load "Domain.fs"
#load "Operations.fs"
#load "Auditing.fs"

open Domain
open Operations
open Auditing
open System

let deposit =
  deposit |> auditAs "deposit" consoleAudit

let withdraw =
  withdraw |> auditAs "withdraw" consoleAudit

let customer = { Name = "Alex" }

let acc =
  { AccountId = Guid.NewGuid()
    Balance = 100M
    Owner = customer }

let test =
  acc
  |> withdraw 50M
  |> deposit 50M
  |> deposit 100M
  |> withdraw 50M
  |> withdraw 350M

let openingAccount =
  { Owner = { Name = "Isaac" }
    Balance = 0M
    AccountId = Guid.Empty }

let isValidCommand command =
  [ 'd'; 'w'; 'x' ] |> Seq.contains command

let isStopCommand command = command = 'x'

let getAmount command =
  match command with
  | 'd' -> ('d', 50M)
  | 'w' -> ('w', 25M)
  | _ -> ('x', 0M)

let processCommand account (command, amount) =
  match command with
  | 'd' -> account |> deposit amount
  | 'w' -> account |> withdraw amount
  | _ -> failwith "Invalid command"
  
let account =
  let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' ]

  commands
  |> Seq.filter isValidCommand
  |> Seq.takeWhile (not << isStopCommand)
  |> Seq.map getAmount
  |> Seq.fold processCommand openingAccount
