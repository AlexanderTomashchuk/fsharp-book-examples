namespace Domain

open System

type Customer = { Name: string }

type Account =
  { AccountId: Guid
    Balance: decimal
    Owner: Customer }

type Transaction =
  { Timestamp: DateTime
    Operation: string
    Amount: decimal
    IsSuccessful: bool }

module Transactions =
  let serialize transaction =
    sprintf "%O***%s***%M***%b" transaction.Timestamp transaction.Operation transaction.Amount transaction.IsSuccessful

  let deserialize (transaction: string) =
    let parts =
      transaction.Split("***", StringSplitOptions.None)

    { Timestamp = parts.[0] |> DateTime.Parse
      Operation = parts.[1]
      Amount = parts.[2] |> Decimal.Parse
      IsSuccessful = parts.[3] |> Boolean.Parse }
