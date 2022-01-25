#load "Domain.fs"
#load "Operations.fs"

open Unit5.Lesson24.Operations
open Unit5.Lesson24.Domain
open System

type Command =
  | Deposit
  | Withdraw
  | Exit

let tryParseCommand str =
  match str with
  | "d" -> Some Deposit
  | "w" -> Some Withdraw
  | "x" -> Some Exit
  | _ -> None

let test1 = "f" |> tryParseCommand
let test2 = "d" |> tryParseCommand
let test3 = "w" |> tryParseCommand
let test4 = "x" |> tryParseCommand

let tryGetAmount command =
  Console.WriteLine()
  Console.Write "Enter amount:"
  let amount = Console.ReadLine() |> Decimal.TryParse

  match amount with
  | true, amount -> Some(command, amount)
  | false, _ -> None

let test5 = "w" |> tryGetAmount
let test6 = "w" |> tryGetAmount

let classifyAccount account =
  match account.Balance >= 0M with
  | true -> InCredit(CreditAccount account)
  | false -> Overdrawn account

let deposit amount account =
  let account =
    match account with
    | InCredit (CreditAccount account) -> account
    | Overdrawn account -> account

  { account with
      Balance = account.Balance + amount }
  |> classifyAccount

let withdraw amount (CreditAccount account) =
  let account =
    match amount <= 0M with
    | true -> account
    | false ->
      { account with
          Balance = account.Balance - amount }

  account |> classifyAccount

let withdrawSafe amount ratedAccount =
  match ratedAccount with
  | InCredit account -> account |> withdraw amount
  | Overdrawn _ -> printfn "Your account is overdrawn - withdrawal rejected!"; ratedAccount
  
let account =
  { Balance = 0M
    AccountId = Guid.NewGuid()
    Owner = { Name = "Alex" } } |> classifyAccount

account
|> deposit 50M
|> deposit 100M
|> withdrawSafe 50M
