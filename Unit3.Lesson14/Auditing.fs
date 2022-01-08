module Auditing

open Domain
open System.IO

let fileSystemAudit account message =
  let path =
    $"%s{account.Customer.Name}-%s{account.Id.ToString()}.txt"

  File.AppendAllText(path, $"\n%s{message}")

let consoleAudit account message =
  printfn $"Account %s{account.Id.ToString()}: %s{message}"

//let withdrawWithConsoleAudit = auditAs "withdraw" consoleAudit withdraw
//let depositWithConsoleAudit = auditAs "deposit" consoleAudit deposit
//let withdrawWithFileSystemAudit = auditAs "withdraw" fileSystemAudit withdraw
//let depositWithFileSystemAudit = auditAs "deposit" fileSystemAudit deposit
