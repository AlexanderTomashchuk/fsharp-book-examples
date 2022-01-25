module Unit5.Lesson24.Operations

open System
open Unit5.Lesson24.Domain

let private classifyAccount account =
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

/// Runs some account operation such as withdraw or deposit with auditing.
let auditAs operationName audit operation amount account accountId owner =
  let updatedAccount = operation amount account

  let transaction =
    { Operation = operationName
      Amount = amount
      Timestamp = DateTime.UtcNow }

  audit accountId owner.Name transaction
  updatedAccount

let tryParseSerializedOperation op =
  match op with
  | "deposit" -> Some Deposit
  | "withdraw" -> Some Withdraw
  | _ -> None

/// Creates an account from a historical set of transactions
let loadAccount (owner, accountId, transactions) =
  let openingAccount =
    { AccountId = accountId
      Balance = 0M
      Owner = { Name = owner } }
    |> classifyAccount

  transactions
  |> Seq.sortBy (fun txn -> txn.Timestamp)
  |> Seq.fold
       (fun account txn ->
         let operation =
           txn.Operation |> tryParseSerializedOperation

         match operation, account with
         | Some Deposit, _ -> account |> deposit txn.Amount
         | Some Withdraw, InCredit creditAccount -> creditAccount |> withdraw txn.Amount
         | Some Withdraw, Overdrawn _ -> account
         | None, _ -> account)
       openingAccount
