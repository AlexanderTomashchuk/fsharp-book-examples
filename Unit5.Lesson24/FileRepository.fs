module Unit5.Lesson24.FileRepository

open Unit5.Lesson24.Domain
open System.IO
open System

let private accountsPath =
  let path = "accounts"
  Directory.CreateDirectory path |> ignore
  path

let private tryFindAccountFolder owner =
  let folders =
    Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner) |> List.ofSeq

  match folders with
  | [] -> None
  | folder :: _ -> Some (DirectoryInfo(folder).Name)

let private buildPath owner (accountId: Guid) =
  sprintf $"%s{accountsPath}/%s{owner}_%O{accountId}"

let loadTransactions (folder: string) =
  let owner, accountId =
    let parts = folder.Split '_'
    parts.[0], Guid.Parse parts.[1]

  owner,
  accountId,
  buildPath owner accountId
  |> Directory.EnumerateFiles
  |> Seq.map (File.ReadAllText >> Transactions.deserialize)

/// Finds all transactions from disk for specific owner.
let tryFindTransactionsOnDisk = tryFindAccountFolder >> Option.map loadTransactions

/// Logs to the file system
let writeTransaction accountId owner transaction =
  let path = buildPath owner accountId
  path |> Directory.CreateDirectory |> ignore

  let filePath =
    sprintf $"%s{path}/%d{transaction.Timestamp.ToFileTimeUtc()}.txt"

  let line = transaction |> Transactions.serialize
  
  File.WriteAllText(filePath, line)
