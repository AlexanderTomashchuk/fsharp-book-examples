
namespace Model

type Car = {
  Wheels: int
  Brand: string option
  Dimensions: float * float
}

type Vehicle =
  | Motorcar of Car
  | Motorbike of Name:string * EngineSize:float

module Functions =
  let CreateCar wheels brand x y =
    { Wheels = wheels; Brand = brand; Dimensions = (x, y) }
  
  let CreateFourWheeledCar = CreateCar 4

