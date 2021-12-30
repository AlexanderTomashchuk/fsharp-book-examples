open System.IO

let text = "Hello, world1"
text.Length

let greetPerson name age =
  sprintf $"Hello, %s{name}. You are %d{age} years old"

let countWords (text:string) =
  let count = text.Split(" ").Length
  
  let file = File.AppendText("countwords.txt")
  file.WriteLine($"%s{text} = %d{count}")
  file.Close();
  file.Dispose()
  
  count

let count = countWords "Hello, my name is Oleksandr Tomashchuk"
  

