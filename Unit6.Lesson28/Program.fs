open System.Collections

type OrderItemRequest = { ItemId: int; Count: int }

type OrderRequest =
  { OrderId: int
    CustomerName: string
    Comment: string
    EmailUpdates: string
    TelephoneUpdates: string
    Items: OrderItemRequest seq }


type OrderId = OrderId of int
type ItemId = ItemId of int
type OrderItem = { ItemId: ItemId; Count: int }

type UpdatePreference =
  | EmailUpdates of string
  | TelephoneUpdates of string

type Order =
  { OrderId: OrderId
    CustomerName: string
    ContactPreference: UpdatePreference option
    Comment: string option
    Items: OrderItem list }

module Functions =
  let validateAndCreateOrder (orderRequest: OrderRequest) =
    { OrderId = OrderId(orderRequest.OrderId)
      CustomerName =
        match orderRequest.CustomerName with
        | null -> failwith "Customer name must be populated"
        | name -> name
      ContactPreference =
        match orderRequest.EmailUpdates |> Option.ofObj, orderRequest.TelephoneUpdates |> Option.ofObj with
        | None, None -> None
        | Some email, None -> Some(EmailUpdates email)
        | None, Some phone -> Some(TelephoneUpdates phone)
        | Some _, Some _ -> failwith ("Unable to proceed - only one of telephone and email should be supplied")
      Comment = orderRequest.Comment |> Option.ofObj
      Items =
        orderRequest.Items
        |> Seq.toList
        |> List.map
             (fun i ->
               { ItemId = ItemId(i.ItemId)
                 Count = i.Count }) }
