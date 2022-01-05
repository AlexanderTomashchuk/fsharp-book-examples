open System
open System.IO

let parseName (name: string) =
  let parts = name.Split(' ')
  let forename = parts.[0]
  let surname = parts.[1]
  forename, surname

let name = parseName "Alex Tom"
let forename, surname = name
let fname, sname = parseName "Alex T"
let snd = snd (parseName "Alex T")
let fst = fst (parseName "Alex T")

let nameAndAge = ("Joe", "Bloggs"), 28
let (fname2, sname2), age2 = nameAndAge

let nameAndAge2 = "Tom", "Jones", 65
let fname3, _, _ = nameAndAge2

//type inference
//arguments type is int * int * a' * b'
let addNumbers arguments =
  let a, b, c, _ = arguments
  a + b

let result, parsed = Int32.TryParse("123")

// poor naming let a, b = getData()
// improved naming let a, b = getBankDetails()
// better naming let a, b = getSortCodeAndAccountNumber()

let getFileNameAndModifiedDate (path: string) =
  let fileName = Path.GetFileName(path)
  let lastModifiedDate = File.GetLastWriteTimeUtc path
  fileName, lastModifiedDate

let fileName, lastModifiedDate =
  getFileNameAndModifiedDate "/Users/at/src/study/docker/first-docker-lesson/hello-world/app.py"
