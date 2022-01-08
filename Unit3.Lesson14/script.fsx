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

let account =
  { Id = Guid.NewGuid()
    Balance = 100M
    Customer = customer }

let test =
  account
  |> withdraw 50M
  |> deposit 50M
  |> deposit 100M
  |> withdraw 50M
  |> withdraw 350M
