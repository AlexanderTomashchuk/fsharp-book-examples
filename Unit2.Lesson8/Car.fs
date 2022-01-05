module Car

let getDistance destination =
  if destination = "Home" then 25
  elif destination = "Office" then 50
  elif destination = "Stadium" then 25
  elif destination = "Gas" then 10
  else failwith "Unknown destination!"

let calculateRemainingPetrol(currentPetrol, distance) =
  let remainingPetrol = currentPetrol - distance
  if remainingPetrol >= 0 then remainingPetrol
  else failwith "You don't have enough petrol"

let driveTo petrol destination =
  let distance = getDistance destination
  let remainingPetrol = calculateRemainingPetrol(petrol, distance)
  
  if destination = "Gas" then remainingPetrol + 50
  else remainingPetrol
