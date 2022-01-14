module Auditing

open Domain

let printCurrentBalance account =
  printfn "Current balance is $%M" account.Balance

let printTransaction _ owner transaction =
  printfn
    "Account %O: %s of %M (approved: %b)"
    owner
    transaction.Operation
    transaction.Amount
    transaction.IsSuccessful

let composedTransaction =
  let loggers = [printTransaction; FileRepository.writeTransaction]
  fun accountId owner transaction ->
    loggers |> List.iter(fun logger -> logger accountId owner transaction)