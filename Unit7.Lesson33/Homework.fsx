#r "nuget: FSharp.Data"

open System
open FSharp.Data
open Microsoft.FSharp.Collections

//HOMEWORK
//Building on a previous “Try this,” create an API that can return the songs for any given Dream Theater album
//by using Wikipedia as a data source. Try returning strings to start with; then build up to creating an explicit
//domain model for Albums and Tracks, hydrating the model from the provided HTML provider types.

[<Measure>]
type sec //second

type Track =
    { Name: string
      Year: int
      Lyricist: string
      Length: float<sec> option }

type Album = { Name: string; Tracks: Track list }

type TimeSpan with
    static member TryParseOption (format: string) str =
        match TimeSpan.TryParseExact(str, format, System.Globalization.CultureInfo.CurrentCulture) with
        | true, ts -> Some ts
        | _ -> None

type DreamTheaterStats = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_songs_recorded_by_Dream_Theater">

let dreamTheaterStats = DreamTheaterStats.GetSample()

let getDreamTheaterStats () = dreamTheaterStats.Tables.ListEdit.Rows

let enrich (rows: DreamTheaterStats.ListEdit.Row seq) =
    rows
    |> Seq.groupBy (fun row -> row.Album)
    |> Seq.map
        (fun (album, rows) ->
            { Name = album
              Tracks =
                  rows
                  |> Seq.map
                      (fun row ->
                          { Name = row.Title
                            Length =
                                (row.Length |> TimeSpan.TryParseOption "m\:ss")
                                |> Option.map (fun ts -> ts.TotalSeconds * 1.0<sec>)
                            Lyricist = row.Lyricist
                            Year = row.Year })
                  |> Seq.toList })
    |> Seq.toList

let loadDreamTheaterStats = getDreamTheaterStats >> enrich

let getYearToAlbumsCount =
    loadDreamTheaterStats
    >> Seq.map (fun album -> (album, album.Tracks |> Seq.map(fun t -> t.Year) |> Seq.min))
    >> Seq.countBy snd
    >> Seq.sortByDescending snd

let getNumbersOfTracksPerYear =
    loadDreamTheaterStats
    >> Seq.collect(fun album -> album.Tracks)
    >> Seq.countBy(fun track -> track.Year)
    >> Seq.sortByDescending snd

let getMostLongestTrack =
    loadDreamTheaterStats
    >> Seq.collect (fun album -> album.Tracks)
    >> Seq.maxBy (fun track -> track.Length)

let mlTrack = getMostLongestTrack ()
