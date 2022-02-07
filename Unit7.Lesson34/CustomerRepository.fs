module CustomerRepository

open FSharp.Data.Sql

[<Literal>]
let private DbVendor = Common.DatabaseProviderTypes.POSTGRESQL

[<Literal>]
let private CompileTimeConnection =
  "Host=localhost;Port=5431;Database=redlink-local-db;Username=root;Password=root;"

[<Literal>]
let private Owner = "public"

type DB = SqlDataProvider<DbVendor, CompileTimeConnection, Owner=Owner>

let private createDbContext (connectionString: string) = DB.GetDataContext(connectionString)

let printProductConditions =
  createDbContext
  >> (fun schema -> schema.Public.ProductCondition)
  >> Seq.iter (fun pc -> printfn "%s" pc.NameLocalizationKey)
