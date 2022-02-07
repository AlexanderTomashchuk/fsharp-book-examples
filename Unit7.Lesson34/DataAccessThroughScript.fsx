#r "nuget: SQLProvider"
#load "CustomerRepository.fs"

let scriptConnectionString =
  "Host=localhost;Port=5431;Database=redlink-local-db;Username=root;Password=root;"

CustomerRepository.printProductConditions scriptConnectionString
