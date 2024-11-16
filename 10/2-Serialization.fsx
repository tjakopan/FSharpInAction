open System.Text.Json
open Microsoft.FSharp.Collections

type Brand = Brand of string

type Strings =
  | Six
  | Seven
  | Eight
  | Twelve

[<RequireQualifiedAccess>]
type Pickup =
  | Single
  | Humbucker

type Kind =
  | Acoustic
  | Electric of Pickup list

type Guitar =
  { Brand: Brand
    Strings: Strings
    Kind: Kind }

let ibanezElectric =
  """
    { "Brand" : "Ibanez",
      "Strings" : "6",
      "Pickups" : [ "H", "S", "H" ] }"""

//System.Text.Json.JsonSerializer.Deserialize<Guitar> ibanezElectric

#r "nuget: FsToolkit.ErrorHandling, 4.18.0"
open FsToolkit.ErrorHandling

type RawGuitar =
  { Brand: string
    Strings: string
    Pickups: string list }

open System

let tryAsFullGuitar (raw: RawGuitar) =
  result {
    let! brand =
      if String.IsNullOrWhiteSpace raw.Brand then
        Error "Brand is mandatory"
      else
        Ok(Brand raw.Brand)

    let! strings =
      match raw.Strings with
      | "6" -> Ok Six
      | "7" -> Ok Seven
      | "8" -> Ok Eight
      | "12" -> Ok Twelve
      | value -> Error $"Invalid value '{value}'"

    let! pickups =
      raw.Pickups
      |> List.traverseResultM (fun pickup ->
        match pickup with
        | "S" -> Ok Pickup.Single
        | "H" -> Ok Pickup.Humbucker
        | value -> Error $"Invalid value {value}")

    return
      { Guitar.Brand = brand
        Guitar.Strings = strings
        Guitar.Kind =
          match pickups with
          | [] -> Acoustic
          | pickups -> Electric pickups }
  }

let ibanezElectricAgain =
  """{ "Brand" : "Ibanez",
"Strings" : "6",
"Pickups" : [ "H", "S", "H" ] }"""

let getOk x = x |> Result.valueOr failwith

let ibanezGuitar =
  ibanezElectricAgain |> JsonSerializer.Deserialize |> tryAsFullGuitar |> getOk

let yamahaAcoustic =
  """{ "Brand" : "Yamaha",
      "Strings" : "6",
      "Pickups" : [] }"""
  |> JsonSerializer.Deserialize
  |> tryAsFullGuitar
  |> getOk

let createReport (guitars: Guitar list) =
  guitars
  |> List.countBy (_.Brand)
  |> List.map (fun (Brand brand, count) -> {| Brand = brand; Guitars = count |})
  |> JsonSerializer.Serialize

[ ibanezGuitar; ibanezGuitar; ibanezGuitar; ibanezGuitar; yamahaAcoustic ]
|> createReport
