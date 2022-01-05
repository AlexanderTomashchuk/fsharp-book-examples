

open System

let getDistance destination =
  if destination = "Home" then 25
  elif destination = "Office" then 50
  elif destination = "Stadium" then 25
  elif destination = "Gas" then 10
  else failwith "Unknown destination!"
  
getDistance("Home") = 25
getDistance("Office") = 50
getDistance("Stadium") = 25
getDistance("Gas") = 10

let calculateRemainingPetrol(currentPetrol, distance) =
  let remainingPetrol = currentPetrol - distance
  if remainingPetrol >= 0 then remainingPetrol
  else failwith "You don't have enough petrol"

calculateRemainingPetrol(100, 50) = 50
calculateRemainingPetrol(50, 25) = 25
calculateRemainingPetrol(25, 10) = 15
calculateRemainingPetrol(15, 20)

let distanceToGas = getDistance "Gas"
calculateRemainingPetrol(25, distanceToGas)
calculateRemainingPetrol(5, distanceToGas)

let driveTo(petrol, destination) =
  let distance = getDistance destination
  let remainingPetrol = calculateRemainingPetrol(petrol, distance)
  
  if destination = "Gas" then remainingPetrol + 50
  else remainingPetrol

let a = driveTo(100, "Office")
let b = driveTo(a, "Stadium")
let c = driveTo(b, "Gas")
let answer = driveTo(c, "Home")