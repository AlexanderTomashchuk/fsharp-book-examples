#r "nuget: FSharp.Data"
#r "nuget: XPlot.GoogleCharts"

open System
open FSharp.Data
open XPlot.GoogleCharts

type UsaCrimes =
  JsonProvider<"https://api.usa.gov/crime/fbi/sapi/api/summarized/state/NV/violent-crime/2010/2020?API_KEY=iiHnOKfno2Mgkt5AynpvPpUQTEyxE77jo1RU8PIv">

let usaCrimes = UsaCrimes.GetSample()

usaCrimes.Results
|> Seq.countBy (fun r -> r.DataYear)
|> Seq.map (fun (year, count) -> (year.ToString(), count))
|> Chart.Pie
|> Chart.WithLegend true
|> Chart.Show

type Films = HtmlProvider<"https://en.wikipedia.org/wiki/Robert_De_Niro_filmography">

let deNiroFilms = Films.GetSample()

deNiroFilms.Tables.FilmsEdit.Rows
|> Array.countBy (fun row -> row.Year)
|> Array.filter (fun (year, _) -> System.Double.IsNaN(year) <> true)
|> Array.map (fun (year, count) -> (year.ToString(), count))
|> Chart.SteppedArea
|> Chart.Show

type AmazonProduct =
  HtmlProvider<"https://www.amazon.com/Snpurdiri-Keyboard-Ultra-Compact-Waterproof-Black-White/dp/B097T276QL/ref=sr_1_2?keywords=gaming+keyboard&pd_rd_r=be525819-bb8c-429f-876f-12e605322aa1&pd_rd_w=3JqNf&pd_rd_wg=aSInm&pf_rd_p=12129333-2117-4490-9c17-6d31baf0582a&pf_rd_r=XGFZK70NT0Z1FG4PEZ40&qid=1643665123&sr=8-2">

let product = AmazonProduct.GetSample()
product.Tables.ProductDetails_detailBullets_sections1.Rows

type AmazonProduct2 =
  HtmlProvider<"https://www.amazon.com/Amazon-Essentials-Womens-Sweatpant-heather/dp/B07BJ83TLD/ref=sr_1_2?keywords=joggers&pd_rd_r=ece5a9fc-b040-48bb-b1b4-2f7852f4a4eb&pd_rd_w=CjpHL&pd_rd_wg=5AyVQ&pf_rd_p=bb27b743-d39d-4423-a18e-dd1d61011f4f&pf_rd_r=JN3QYNPCRC2W8JRMPT0Z&qid=1643665245&s=fashion-womens-intl-ship&sr=1-2">

let product2 = AmazonProduct2.GetSample()
product2.Lists.``Product details``


type Package = HtmlProvider<"https://www.nuget.org/packages/nunit">
let package = Package.GetSample()

let entityFramework =
  Package.Load "https://www.nuget.org/packages/EntityFramework/"

entityFramework.Tables.Table3.Rows
|> Array.map (fun row -> (row.Version, row.Downloads))
|> Array.sortByDescending snd
|> Chart.Bar
|> Chart.Show

type DreamTheaterStats = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_songs_recorded_by_Dream_Theater">
let dreamTheaterStats = DreamTheaterStats.GetSample()

let yearWithMostAlbums =
  dreamTheaterStats.Tables.ListEdit.Rows
  |> Array.map (fun row -> (row.Year, row.Album))
  |> Array.distinct
  |> Array.countBy fst
  |> Array.sortByDescending fst
  |> Chart.Line
  |> Chart.Show

let numbersOfTracksPerYear =
  dreamTheaterStats.Tables.ListEdit.Rows
  |> Array.map(fun row -> (row.Year, row.Title))
  |> Array.countBy fst
  |> Array.sortByDescending fst
  |> Chart.Bar
  |> Chart.Show