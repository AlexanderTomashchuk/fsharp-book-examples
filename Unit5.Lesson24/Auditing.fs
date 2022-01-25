module Unit5.Lesson24.Auditing

open Unit5.Lesson24.Domain

let printCurrentBalance (account: RatedAccount) =
  let balance = account.GetField(fun a -> a.Balance)
  printfn "Current balance is $%M" balance

let printTransaction _ owner transaction =
  printfn "Account %O: %s of %M" owner transaction.Operation transaction.Amount

let composedTransaction =
  let loggers =
    [ printTransaction
      FileRepository.writeTransaction ]

  fun accountId owner transaction ->
    loggers
    |> List.iter (fun logger -> logger accountId owner transaction)
