
let parse (gameDescription:string) =
  let parts = gameDescription.Split(' ')
  let playerName = parts.[0]
  let game = parts.[1]
  let score = int parts.[2]
  playerName, game, score
  
let name, game, score = parse "Mary Asteroids 2500"
