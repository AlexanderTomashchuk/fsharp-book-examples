open System
open FSharp.Data
open Microsoft.FSharp.Collections

module Nuget =
  type Package = HtmlProvider<"https://www.nuget.org/packages/Serilog">

  let nugetService = Package.GetSample()

  type PackageVersion =
    | Release
    | Prerelease

  type VersionDetails =
    { Version: Version
      Downloads: decimal
      PackageVersion: PackageVersion
      LastUpdated: DateTime }

  type NugetPackage =
    { PackageName: string
      Versions: VersionDetails list }

  let parse (versionText: string) =
    let getVersionPart (version: string) =
      match version |> Version.TryParse with
      | true, value -> Some value
      | false, _ -> None

    match versionText.Split("-") |> Seq.toList with
    | [] -> None
    | [ head ] ->
      head
      |> getVersionPart
      |> Option.map (fun version -> (version, Release))
    | head :: _ ->
      head
      |> getVersionPart
      |> Option.map (fun version -> (version, Prerelease))

  let enrich (packageName, rows: Package.Table3.Row []) =
    { PackageName = packageName
      Versions =
        rows
        |> Seq.toList
        |> List.choose
             (fun r ->
               r.Version
               |> parse
               |> Option.map
                    (fun (version, packageVersion) ->
                      { Version = version
                        PackageVersion = packageVersion
                        Downloads = r.Downloads
                        LastUpdated = r.``Last updated`` })) }

  let getPackage packageName =
    packageName
    |> sprintf "https://www.nuget.org/packages/%s"
    |> Package.Load
    |> (fun package -> packageName, package)

  let getVersionsForPackage (packageName, package: Package) = packageName, package.Tables.Table3.Rows

  let loadPackage =
    getPackage >> getVersionsForPackage >> enrich

  let loadPackageVersions = loadPackage >> (fun p -> p.Versions)

  let getDownloadsForPackage =
    loadPackageVersions
    >> Seq.sumBy (fun p -> p.Downloads)

  let packageVersionExists version =
    loadPackageVersions
    >> Seq.exists (fun versionDetails -> versionDetails.Version = version)

  let package1Details = "Serilog" |> loadPackage
  let package1DownloadCount = "Serilog" |> getDownloadsForPackage

  let package1WithVersionExists =
    "Serilog"
    |> packageVersionExists (Version.Parse "2.11.0")
