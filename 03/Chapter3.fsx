let exercise31 a b c =
  let inProgress = a + b
  let answer = inProgress * c
  $"The answer is {answer}"

let greetingTextScoped =
  let fullName =
    let fname = "Frank"
    let sname = "Schmidt"
    $"{fname} {sname}"

  $"Greetings, {fullName}"

let greetingTextWithFunction person =
  let makeFullName fname sname = $"{fname} {sname}"
  let fullName = makeFullName "Frank" "Schmidt"
  $"Greetings {fullName} from {person}"

let greetingTextWithFunctionScoped =
  let city = "London"
  let makeFullName fname sname = $"{fname} {sname} from {city}"
  let fullName = makeFullName "Frank" "Schmidt"
  //let surnameCity = $"{sname}" from {city} // won't compile
  $"Greetings, {fullName}"

let add a b =
  let answer = a + b
  answer

let explicit = ResizeArray<int>()
let typeHole = ResizeArray<_>()
let omitted = ResizeArray()

typeHole.Add 99
omitted.Add "10"

let combineElements<'T> (a: 'T) (b: 'T) (c: 'T) =
  let output = ResizeArray<'T>()
  output.Add a
  output.Add b
  output.Add c

combineElements<int> 1 2 3

let combineElementsNoTypeAnnotations a b c =
  let output = ResizeArray()
  output.Add a
  output.Add b
  output.Add c
  output

combineElementsNoTypeAnnotations 1 2 3

let calculateGroup age =
  if age < 18 then "Child"
  elif age < 65 then "Adult"
  else "Pensioner"

let sayHello someValue =
  let group =
    if someValue < 10.0 then
      calculateGroup 15
    else
      calculateGroup 35

  "Hello " + group

let result = sayHello 10.5

let addThreeDays (theDate: System.DateTime) = theDate.AddDays 3

let aaAYearAndThreeDays theDate =
  let threeDaysForward = addThreeDays theDate
  theDate.AddYears 1
