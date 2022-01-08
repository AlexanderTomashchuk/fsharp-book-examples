module Operations

open Domain

let deposit amount account =
  let newBalance = account.Balance + amount

  if amount <= 0.0M then
    account
  else
    { account with Balance = newBalance }

let withdraw amount account =
  let newBalance = account.Balance - amount

  if amount <= 0.0M then
    account
  elif newBalance < 0.0M then
    account
  else
    { account with Balance = newBalance }

let auditAs operationName audit operation amount account =
  let changedAccount = operation amount account

  $"Performing a %s{operationName} operation for $%M{amount}..."
  |> audit account

  if changedAccount <> account then
    $"Transaction accepted! Balance is now $%M{changedAccount.Balance}."
    |> audit account
  else
    "ERROR: Transaction rejected!" |> audit account

  changedAccount
