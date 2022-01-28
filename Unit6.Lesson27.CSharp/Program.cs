using Microsoft.FSharp.Core;
using Model;

var car = new Car(4, "BMW", new Tuple<double, double>(5.0, 6.0));

var bike = Vehicle.NewMotorbike("Bike", 1.2);

Console.WriteLine(bike.IsMotorbike);

var someWheeledCar = Functions.CreateCar(4, "Opel", 5.0, 6.0);

var fourWheeledCar = Functions.CreateFourWheeledCar.Invoke("Mercedes").Invoke(5.0).Invoke(6.0);

Console.WriteLine(fourWheeledCar.Brand);

var carWithOptionField = new Car(4, FSharpOption<string>.None, new Tuple<double, double>(3, 2));
