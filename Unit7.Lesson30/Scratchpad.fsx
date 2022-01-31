//#I "/Users/at/.nuget/packages"
//#r "./fsharp.data/4.2.7/lib/netstandard2.0/FSharp.Data.dll"
#r "nuget: FSharp.Data"
#r "nuget: XPlot.GoogleCharts"

open FSharp.Data
open XPlot.GoogleCharts

type Football = CsvProvider< @"/Users/at/src/study/fsharp-book-examples/Unit7.Lesson30/data/FootballResults.csv" >
let data = Football.GetSample().Rows |> Seq.toArray

data
|> Seq.filter (fun row -> row.``Full Time Home Goals`` > row.``Full Time Away Goals``)
|> Seq.countBy (fun row -> row.``Home Team``)
|> Seq.sortByDescending snd
|> Seq.take 10
|> Chart.Column
|> Chart.Show

let teamsWithMostGoals =
    data
    |> Seq.collect
        (fun row ->
            [ (row.``Home Team``, row.``Full Time Home Goals``)
              (row.``Away Team``, row.``Full Time Away Goals``) ])
    |> Seq.groupBy fst
    |> Seq.map(fun (group, items) -> (group, items |> Seq.sumBy snd))
    |> Seq.sortByDescending snd
    |> Seq.take 5
    |> Chart.Pie
    |> Chart.WithLegend true
    |> Chart.WithSize (500, 500)
    |> Chart.Show
