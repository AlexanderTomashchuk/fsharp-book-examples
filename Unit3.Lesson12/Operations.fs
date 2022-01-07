//module Domain.Operations
[<Microsoft.FSharp.Core.AutoOpen>]
module Operations

open Domain

let getInitials customer =
  $"%s{customer.FirstName.[0].ToString()}. %s{customer.LastName.[0].ToString()}."

let isOrderThan age customer = customer.Age > age
