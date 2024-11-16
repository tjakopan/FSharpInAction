open System
open System.Text.RegularExpressions
open Microsoft.FSharp.Core

let optionalNumber: int option = Some 10
let missingNumber: int option = None
let mandatoryNumber: int = 10

mandatoryNumber.CompareTo 55
// missingNumber.CompareTo 55
// missingNumber + mandatoryNumber

let description =
  match optionalNumber with
  | Some number -> $"The number is {number}"
  | None -> "There is no number"

type Customer = { Age: int }

let getAge customer : int option =
  match customer with
  | Some c -> Some c.Age
  | None -> None

let getAge2 c = c.Age
let getAgeOptional customer : int option = customer |> Option.map getAge2

let classifyCustomer c = if c < 18 then "Child" else "Adult"

let optionalClassification optionalCustomer =
  optionalCustomer |> Option.map getAge2 |> Option.map classifyCustomer

let optionalClassificationShort optionalCustomer =
  optionalCustomer |> Option.map (getAge2 >> classifyCustomer)

let tryGetFileContents filePath =
  if System.IO.File.Exists filePath then
    Some(System.IO.File.ReadAllText filePath)
  else
    None

let countWords (text: string) =
  Regex.Split(text, @"\b")
  |> Array.filter (fun s -> s.Trim() <> "")
  |> Array.length

let countWordsInFile filePath =
  filePath |> tryGetFileContents |> Option.map countWords

type CustomerOptionalAge = { Name: string; Age: int option }
let getAge3 c = c.Age
let theCustomer = Some { Name = "Isaac"; Age = Some 42 }
let optionalAgeDoubleOpt: int option option = theCustomer |> Option.map getAge3
let optionalAge: int option = theCustomer |> Option.bind getAge3

let optionalName: string option = null |> Option.ofObj
let optionalNameTwo = "Issac" |> Option.ofObj
let optionalAge2: int option = Nullable() |> Option.ofNullable
let optionalAge3 = Nullable 123 |> Option.ofNullable
