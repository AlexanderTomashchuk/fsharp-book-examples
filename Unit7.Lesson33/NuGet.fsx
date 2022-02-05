#r "nuget: FSharp.Data"

open FSharp.Data
open Microsoft.FSharp.Core

//type NuGet = FSharp.Data.HtmlProvider<"/Users/at/src/study/fsharp-book-examples/unit7.lesson33/data/sample-package.html">
type NuGet = HtmlProvider<"https://www.nuget.org/packages/Serilog">

let nugetService = NuGet.GetSample()

let getPackage pkgName =
  pkgName |> sprintf "https://www.nuget.org/packages/%s" |> NuGet.Load

let getDownloadsForPackage pkgName =
  let pkg = getPackage pkgName
  pkg.Tables.Table3.Rows
  |> Seq.sumBy(fun row -> row.Downloads)

let versionExists (versionText:string) pkgName =
  let pkg = getPackage pkgName
  pkg.Tables.Table3.Rows
  |> Seq.tryFind(fun row -> row.Version.Contains versionText)
  |> Option.isSome

let newtonsoftJson = getDownloadsForPackage "Newtonsoft.JSON"
let serilog = getDownloadsForPackage "Serilog"
let serilog2exists = "Serilog" |> versionExists "2.0.0"
