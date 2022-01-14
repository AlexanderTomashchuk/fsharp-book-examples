module Operations

open System
open Domain

let deposit amount account =
  if amount <= 0.0M then
    account
  else
    { account with
        Balance = account.Balance + amount }

let withdraw amount account =
  let newBalance = account.Balance - amount

  if amount <= 0.0M then
    account
  elif newBalance < 0.0M then
    account
  else
    { account with Balance = newBalance }

let auditAs operationName audit operation amount account =
  let updatedAccount = operation amount account

  let accountIsUnchanged = (account = updatedAccount)

  let transaction =
    let transaction =
      { Operation = operationName
        Amount = amount
        Timestamp = DateTime.UtcNow
        IsSuccessful = true }

    if accountIsUnchanged then
      { transaction with
          IsSuccessful = false }
    else
      transaction

  audit account.AccountId account.Owner.Name transaction

  updatedAccount

let loadAccount (owner, accountId, transactions) =
  let openingAccount =
    { AccountId = accountId
      Balance = 0M
      Owner = { Name = owner } }

  transactions
  |> Seq.sortBy (fun transaction -> transaction.Timestamp)
  |> Seq.fold
       (fun account transaction ->
         if transaction.Operation = "deposit" then
           account |> deposit transaction.Amount
         else
           account |> withdraw transaction.Amount)
       openingAccount
