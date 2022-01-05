
let mutable name = "Alex"
//name = "Tom"
name <- "Tom"

let mutable petrol = 100.0
let drive(distance) =
  if distance = "far" then petrol <- petrol / 2.0
  elif distance = "medium" then petrol <- petrol - 10.0
  else petrol <- petrol - 1.0

drive("far")
drive("medium")
drive("short")
  
petrol

let drive2(petrol, distance) =
  if distance > 50 then petrol / 2.0
  elif distance > 25 then petrol - 10.0
  elif distance > 0 then petrol - 1.0
  else petrol

let petrol2 = 100.0
let firstState = drive2(petrol2, 100)
let secondState = drive2(firstState, 30)
let thirdState = drive2(secondState, 10)
let forthState = drive2(thirdState, 0)
let fifthState = drive2(forthState, 0)
fifthState



