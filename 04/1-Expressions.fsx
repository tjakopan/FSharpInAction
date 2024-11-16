let ageDescription age =
  if age < 18 then "Child"
  elif age < 65 then "Adult"
  else "OAP"

let describeAge age =
  let ageDescription = ageDescription age
  let greeting = "Hello"
  $"{greeting}! You are a '{ageDescription}'."

let printAddition a b =
  let answer = a + b
  printfn $"{a} plus {b} equals {answer}"

let addDays days =
  let newDays = System.DateTime.Today.AddDays days
  printfn $"You gave me {days} days and I gave you {newDays}"
  newDays

let result = addDays 3

let addSeveralDays () =
  addDays 3 |> ignore
  addDays 5 |> ignore
  addDays 7
