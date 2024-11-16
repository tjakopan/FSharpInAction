open System

let numbersArray = [| 1..10 |]
let secondElementArray = numbersArray[1]
let squaresArray = numbersArray |> Array.map (fun x -> x * x)

let numbersSeq = seq { 1..10 }
let secondElementSeq = numbersSeq |> Seq.item 1
let squaresSeq = numbersSeq |> Seq.map (fun x -> x * x)

let data = [ "Isaac", "F#"; "Fred", "C#"; "Sam", "F#"; "Jo", "PHP" ]
let lookup = readOnlyDict data
let isaacsLang = lookup["Isaac"]

let lookupMap = Map data
let newLookupMap = lookupMap.Add("Isaac", "Python")
let newLookupMap2 = lookupMap |> Map.add "Isaac" "Python"

type Employee = { Name: string }
let salesEmployees = Set [ { Name = "Isaac" }; { Name = "Brian" } ]
let bonusEmployees = Set [ { Name = "Isaac" }; { Name = "Tanya" } ]
let allBonusesForSalesStaff = salesEmployees |> Set.isSubset bonusEmployees
let salesWithoutBonuses = salesEmployees - bonusEmployees

#time

let largeComputation =
  seq { 1..100000000 }
  |> Seq.map (fun x -> x * x)
  |> Seq.rev
  |> Seq.filter (fun x -> x % 2 = 0)
  |> Seq.toArray

let comp =
  seq {
    1
    2

    if DateTime.Today.DayOfWeek = DayOfWeek.Tuesday then
      99

    4
  }
