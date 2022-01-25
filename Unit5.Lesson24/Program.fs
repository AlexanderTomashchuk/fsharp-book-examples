module Unit5.Lesson24.Program

open System
open Unit5.Lesson24.Domain
open Unit5.Lesson24.Operations

let depositWithAudit amount (ratedAccount: RatedAccount) =
  let accountId =
    ratedAccount.GetField(fun a -> a.AccountId)

  let owner = ratedAccount.GetField(fun a -> a.Owner)

  auditAs "deposit" Auditing.composedTransaction deposit amount ratedAccount accountId owner

let withdrawWithAudit amount (CreditAccount account as creditAccount) =
  auditAs "withdraw" Auditing.composedTransaction withdraw amount creditAccount account.AccountId account.Owner

let tryLoadAccountFromDisk =
  FileRepository.tryFindTransactionsOnDisk
  >> Option.map Operations.loadAccount

type Command =
  | AccountCmd of BankOperation
  | Exit

[<AutoOpen>]
module CommandParsing =
  let tryParseCommand cmd =
    match cmd with
    | "d" -> Some(AccountCmd Deposit)
    | "w" -> Some(AccountCmd Withdraw)
    | "x" -> Some Exit
    | _ -> None

  let isValidCommand cmd = cmd |> tryParseCommand

  let isStopCommand cmd = cmd = Exit

[<AutoOpen>]
module UserInput =
  let commands =
    seq {
      while true do
        printfn "(d)eposit/(w)ithdraw/e(x)it:"
        yield Console.ReadLine()
        printfn ""
    }

  let tryGetAmount command =
    printfn "Enter amount:"

    let amount = Console.ReadLine() |> Decimal.TryParse

    match amount with
    | true, amount when amount <= 0M -> None
    | false, _ -> None
    | true, amount -> Some(command, amount)

[<EntryPoint>]
let main _ =
  let openingAccount =
    printf "Please enter your name: "
    let owner = Console.ReadLine()

    match owner |> tryLoadAccountFromDisk with
    | Some account -> account
    | None ->
      InCredit(
        CreditAccount
          { Balance = 0M
            AccountId = Guid.NewGuid()
            Owner = { Name = owner } }
      )

  Auditing.printCurrentBalance openingAccount

  let processCommand account (command, amount) =
    let account =
      match command with
      | Deposit -> account |> depositWithAudit amount
      | Withdraw ->
        match account with
        | InCredit creditAccount -> creditAccount |> withdrawWithAudit amount
        | Overdrawn _ ->
          printfn "You cannot withdraw funds as your account is overdrawn!!"
          account

    Auditing.printCurrentBalance account

    match account with
    | InCredit _ -> ()
    | Overdrawn _ -> printfn "Your account is overdrawn!!"

    account

  let closingAccount =
    commands
    |> Seq.choose isValidCommand
    |> Seq.takeWhile ((<>) Exit)
    |> Seq.choose
         (fun cmd ->
           match cmd with
           | Exit -> None
           | AccountCmd cmd -> Some cmd)
    |> Seq.choose tryGetAmount
    |> Seq.fold processCommand openingAccount

  printfn $"Closing Balance:\r\n %A{closingAccount}"
  printfn "Enter any key to exit..."
  Console.ReadKey() |> ignore

  0
