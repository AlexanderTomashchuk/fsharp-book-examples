open FSharp.Data

let [<Literal>] FootballResultsPath = __SOURCE_DIRECTORY__ + "/data/FootballResults.csv"

type Football = CsvProvider<FootballResultsPath>

let data = Football.GetSample().Rows |> Seq.toArray
