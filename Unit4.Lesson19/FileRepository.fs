module FileRepository

open System
open System.IO
open Domain

let private accountsPath =
  let path = "accounts"
  path |> Directory.CreateDirectory |> ignore
  path

let private findAccountFolder owner =
  let folders =
    Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner)

  if Seq.isEmpty folders then
    ""
  else
    let folder = Seq.head folders
    DirectoryInfo(folder).Name

let private buildPath owner (accountId: Guid) =
  sprintf $"%s{accountsPath}/%s{owner}_%O{accountId}"

let writeTransaction accountId owner transaction =
  let path = buildPath owner accountId
  path |> Directory.CreateDirectory |> ignore

  let filePath =
    sprintf $"%s{path}/%d{transaction.Timestamp.ToFileTimeUtc()}.txt"

  let line = transaction |> Transactions.serialize

  File.WriteAllText(filePath, line)

let loadTransactions (folder: string) =
  let owner, accountId =
    let parts = folder.Split "_"
    parts.[0], parts.[1] |> Guid.Parse

  owner,
  accountId,
  buildPath owner accountId
  |> Directory.EnumerateFiles
  |> Seq.map (File.ReadAllText >> Transactions.deserialize)

let findTransactionsOnDisk owner =
  let folder = findAccountFolder owner

  if String.IsNullOrEmpty folder then
    owner, Guid.NewGuid(), Seq.empty
  else
    folder |> loadTransactions
