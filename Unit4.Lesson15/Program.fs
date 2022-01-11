type FootballResult =
  { HomeTeam: string
    AwayTeam: string
    HomeGoals: int
    AwayGoals: int }

let create (ht, hg) (at, ag) =
  { HomeTeam = ht
    AwayTeam = at
    HomeGoals = hg
    AwayGoals = ag }

let results =
  [ create ("Messiville", 1) ("Ronaldo City", 2)
    create ("Messiville", 1) ("Bale Town", 3)
    create ("Bale Town", 3) ("Ronaldo City", 1)
    create ("Bale Town", 2) ("Messiville", 1)
    create ("Ronaldo City", 4) ("Messiville", 2)
    create ("Ronaldo City", 1) ("Bale Town", 2) ]

let teamsWithMostAwayWins =
  results
  |> List.filter (fun r -> r.AwayGoals > r.HomeGoals)
  |> List.countBy (fun r -> r.AwayTeam)
  |> List.sortByDescending (fun (_, awayWins) -> awayWins)

//arrays ARE MUTABLE
let numbersArray = [| 1; 2; 3; 4; 5 |]
let firstNumber = numbersArray.[0]
let firstThreeNumbers = numbersArray.[0..2]
// mutating the value of an item in an array
numbersArray.[0] <- 99

//lists are IMMUTABLE
//internally lists are linked lists
let numbers = [ 1; 2; 3; 4; 5; 6 ]
let numbersQuick = [ 1 .. 6 ]
let head::tail = numbers
let moreNumbers = 0 :: numbers
let evenMoreNumbers = moreNumbers @ [ 7 .. 9 ]
let moreNum = numbers @ [ 100 ]

//sequences are IMMUTABLE
let seq1 = seq { 1 .. 10 .. 100 }
let seq2 = seq { for i in 1 .. 10 -> i * i }
let seq3 = seq { for i in 1 .. 10 do yield i * i }
let seq4 = seq { for i in 1 .. 10 do i * i }
let isPrime (n:int) = true
let seq5 = seq { for n in 1 .. 10 do if isPrime n then n }

//include seq inside another seq - use yield! keyword
let seq6 = seq { for _ in 1 .. 10 do yield! seq {1;2;3;4;5;} } // Repeats '1 2 3 4 5' ten times
seq6 |> Seq.iter (fun i -> printfn $"%A{i}")
let seq7 = seq { // Combine repeated values with their values
  for x in 1 .. 10 do
    yield x
    yield! seq {for i in 1..x -> i}
}
seq7 |> Seq.iter (fun i -> printfn $"%A{i}")
