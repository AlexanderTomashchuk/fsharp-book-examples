open System
open Microsoft.FSharp.Core

type Disk =
  | HardDisk of RPM: int * Platters: int
  | SolidState
  | MMC of NumberOfPins: int

type Computer =
  { Manufacturer: string
    Disks: Disk list }

let myHardDisk = HardDisk(RPM = 250, Platters = 7)
let myHardDiskShort = HardDisk(250, 7)
let args = 250, 7
let myHardDiskTupled = HardDisk args
let myMMC = MMC 5
let mySSD = SolidState

let myPc =
  { Manufacturer = "Computer Inc."
    Disks = [ myHardDisk; myMMC; mySSD ] }

let seek disk =
  match disk with
  | HardDisk (RPM, Platters) -> sprintf "Seeking loudly at a reasonable speed! RPM: %d; Platters: %d;" RPM Platters
  | MMC _ -> "Seeking quietly but slowly"
  | SolidState _ -> "Already found it!"

seek mySSD
seek myHardDisk

let describe disk =
  match disk with
  | SolidState _ -> "I'm a newfangled SSD."
  | MMC 1 -> "I have only 1 pin."
  | MMC pins when pins > 5 -> "I'm MMC with a few pins."
  | MMC pins -> sprintf "I'm an MMC with %d pins." pins
  | HardDisk (5400, _) -> "I'm a slow hard disk."
  | HardDisk (_, 7) -> "I have 7 spindles!"
  | HardDisk _ -> "I'm a hard disk."

let newSSD = SolidState
let newMMC = MMC 4
let newHardDisk = HardDisk(8400, 1)

describe newHardDisk

//nested DUs
type MMCDisk =
  | RsMCC
  | MMCPlus
  | SecureMMC

type Disk2 =
  | MMC of MMCDisk * NumberOfPins: int
  | HardDisk of RPM: int * Platters: int
  | SolidState

let testMMCDisk disk =
  match disk with
  | MMC (MMCPlus, 3) -> "Seeking quietly but slowly"
  | MMC (SecureMMC, 6) -> "Seeking quietly with 6 pins"
  | _ -> failwith "todo"

let newMMC2 = MMC(MMCPlus, 3)
testMMCDisk newMMC2

type DiskInfo =
  { Manufacturer: string
    SizeGb: int
    DiskData: Disk2 }

type Computer2 =
  { Manufacturer: string
    Disks: DiskInfo list }

let myPc2 =
  { Manufacturer = "Computers Inc."
    Disks =
      [ { Manufacturer = "HardDisks Inc."
          SizeGb = 100
          DiskData = HardDisk(5400, 7) }
        { Manufacturer = "SuperDisks Corp."
          SizeGb = 250
          DiskData = SolidState } ] }

sprintf "%A" newMMC2

//creating enums
type Printer =
  | Injket = 0
  | Laserjet = 1
  | DotMatrix = 2

let print (printer: Printer) =
  match printer with
  | Printer.Injket -> "Injket prints..."
  | Printer.Laserjet -> "Laserjet prints..."
  | Printer.DotMatrix -> "DotMatrix prints..."
  | _ -> ArgumentOutOfRangeException() |> raise

let s = print Printer.Injket
let notExistentPrinter = enum<Printer> (5)
let s2 = print notExistentPrinter

//HOMEWORK
//try to update the rules engine you looked at earlier in the book, so that instead of returning a tuple
//of the rule name and the error, it returns a Pass or Fail discriminated union, with the failure case
//containing the error message.

type RuleValidationResult =
  | Pass
  | Fail of Msg: string

type Rule = string -> RuleValidationResult

let rules: Rule list =
  [ fun text ->
      match (text.Split ' ').Length = 3 with
      | true -> Pass
      | false -> Fail "Must be three words"
    fun text ->
      match text.Length <= 30 with
      | true -> Pass
      | false -> Fail "Max length is 30 characters"
    fun text ->
      match text
            |> Seq.filter Char.IsLetter
            |> Seq.forall Char.IsUpper with
      | true -> Pass
      | false -> Fail "All letters must be caps"
    fun text ->
      match text
            |> Seq.forall (fun n -> Char.IsNumber(n) = false) with
      | true -> Pass
      | false -> Fail "Should not contain numbers" ]

let buildValidator (rules: Rule list) =
  rules
  |> List.reduce
       (fun firstRule secondRule ->
         fun word ->
           let validationResult = firstRule word

           let combinedValidationResult =
             match validationResult with
             | Pass ->
               let validationResult = secondRule word
               validationResult
             | Fail _ -> validationResult

           combinedValidationResult)

let validate = buildValidator rules
let word = "HELLO FROM F#FF"
let validationResult = word |> validate
