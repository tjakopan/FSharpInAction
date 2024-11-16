let theAuthor = "isaac", "abraham"
let firstName, secondName = theAuthor

let name = "isaac", "abraham", 42, "london"

let nameAndAge = "Jane", "Smith", 25
let forename, surname, _ = nameAndAge

let makeDoctor name =
  let _, sname = name
  "Dr", sname

let makeDoctorDecon (_, sname) = "Dr", sname

let nameAndAgeNested = ("Joe", "Bloggs"), 28
let nameNested, age = nameAndAgeNested
let (forenameNested, surnameNested), theAge = nameAndAgeNested

let buildPerson (forename: string) (surname: string) (age: int) =
  (forename, surname), (age, (if age < 18 then "child" else "adult"))

let makeDoctorStruct (name: struct (string * string)) =
  let struct (_, sname) = name
  struct ("Dr", sname)
  
let parsed, theDate = System.DateTime.TryParse "03 Dec 2020"
if parsed then
  printfn $"Day of the week is {theDate.DayOfWeek}"
