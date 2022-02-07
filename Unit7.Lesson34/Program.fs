open System.Configuration

[<EntryPoint>]
let main _ =
  let runtimeConnectionString =
    ConfigurationManager.ConnectionStrings.["RedlinkDB"]
      .ConnectionString

  CustomerRepository.printProductConditions runtimeConnectionString

  0
