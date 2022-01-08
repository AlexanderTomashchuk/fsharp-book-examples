open System
open Domain
open Operations

[<EntryPoint>]
let main argv =
  printfn "Enter the customer name:"
  let customerName = Console.ReadLine()
  printfn "Enter the opening balance:"
  let balance = Console.ReadLine() |> Decimal.Parse

  let mutable account =
    { Id = Guid.NewGuid()
      Balance = balance
      Customer = { Name = customerName } }

  let depositWithAudit =
    deposit
    |> auditAs "deposit" Auditing.fileSystemAudit

  let withdrawWithAudit =
    withdraw
    |> auditAs "withdraw" Auditing.fileSystemAudit

  while true do
    printfn "Choose the operation on account ('d' - deposit; 'w' - withdraw; 'q' - quit;):"
    let action = Console.ReadLine()
    if action = "q" then Environment.Exit 0

    printfn "Enter the amount:"
    let amount = Console.ReadLine() |> Decimal.Parse

    account <-
      if action = "d" then
        depositWithAudit amount account
      elif action = "w" then
        withdrawWithAudit amount account
      else
        account

  0
