#r "nuget: SQLProvider"
#r "nuget: XPlot.GoogleCharts"

open System
open FSharp.Data.Sql
open XPlot.GoogleCharts

[<Literal>]
let dbVendor = Common.DatabaseProviderTypes.POSTGRESQL

[<Literal>]
let connectionString =
  "Host=localhost;Database=Adventureworks;Username=postgres;Password=postgres"

[<Literal>]
let owner =
  "public, admin, references, sa, sales, person, humanresources"

type Sql = SqlDataProvider<dbVendor, connectionString, Owner=owner>

let context = Sql.GetDataContext()

let creditCardsChart =
  context.Sales.Creditcard
  |> Seq.map (fun cc -> (cc.Cardtype, cc.Cardnumber))
  |> Seq.countBy fst
  |> Chart.Pie
  |> Chart.WithLegend true
  |> Chart.Show

let customers = context.Sales.Customer |> Seq.toArray

let persons = context.Person.Person |> Seq.toArray

let test =
  context.Humanresources.Employee |> Seq.toArray

let addresses = context.Person.Address |> Seq.toArray
let newAddress = context.Person.Address.Create()
//  context.Person.Address.``Create(addressline1, city, postalcode, stateprovinceid)`` ("Pere", "Kyiv", "02217", 7)

newAddress.Addressline1 <- "Peremohy ave 141"
newAddress.City <- "Kyiv"
newAddress.Postalcode <- "02217"
newAddress.Stateprovinceid <- 7
newAddress.Addressid <- 32522
context.SubmitUpdates()

let first10Addresses =
  query {
    for address in context.Person.Address do
    sortBy address.Addressid
    take 10
  }
  |> Seq.toArray
  |> Array.iter(fun address -> printfn "%d, %s, %s" address.Addressid address.Addressline1 address.City )
