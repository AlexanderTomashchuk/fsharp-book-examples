type Customer = { Age: int }

let where filter customers =
  seq {
    for customer in customers do
      if filter customer then yield customer
  }

let customers =
  [ { Age = 15 }
    { Age = 30 }
    { Age = 60 } ]

let isOver35 customer = customer.Age > 35

customers |> where isOver35
let oldCustomers2 = customers |> where (fun customer -> customer.Age > 35)
