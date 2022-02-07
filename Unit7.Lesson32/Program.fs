open FSharp.Data.Sql

[<Literal>]
let dbVendor = Common.DatabaseProviderTypes.POSTGRESQL

[<Literal>]
let connectionString =
    "Host=localhost;Database=Adventureworks;Username=postgres;Password=postgres"

[<Literal>]
let owner = "public, admin, references, sa, sales"

type Sql = SqlDataProvider<dbVendor, connectionString, Owner=owner>

let sql = Sql.GetDataContext(connectionString)

let salesPersons = sql.Sales.Salesperson|> Seq.toArray
