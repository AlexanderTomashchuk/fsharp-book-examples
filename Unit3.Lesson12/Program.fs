open Domain
//autoopenned module open Operations

[<EntryPoint>]
let main argv =
  let joe =
    { FirstName = "Joe"
      LastName = "Rottellaa"
      Age = 40 }

  joe |> getInitials |> printfn "Joe's initials %s"

  if joe |> isOrderThan 18 then
    printfn "%s is adult!" joe.FirstName
  else
    printfn "%s is a child." joe.FirstName

  0
