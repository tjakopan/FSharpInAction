#r "nuget:FsToolkit.ErrorHandling, 4.18.0"

open System
open Microsoft.FSharp.Collections

type TrainId = TrainId of string
type Station = Station of string
type CarriageId = CarriageId of int

type CarriageClass =
  | First
  | Second

type CarriageType =
  | Passenger of CarriageClass
  | Buffet of
    {| HotFood: Boolean
       ColdFood: Boolean |}

type CarriageFeature =
  | Quiet
  | Wifi
  | Washroom

type Carriage =
  { Id: CarriageId
    Type: CarriageType
    NumberOfSeats: int
    Features: CarriageFeature Set }

type Stop = { Station: Station; Arrival: TimeOnly }

type Train =
  { Id: TrainId
    Origin: Stop
    Stops: Stop list
    Destination: Stop
    DriverChange: Station option
    Carriages: Carriage list }

module TrainFunctions =
  let numberOfSeats train =
    train.Carriages |> List.sumBy _.NumberOfSeats

  type TimeToTravelError =
    | StartAndEndStationAreTheSame of Station
    | InvalidOrderOfStations of Station * Station
    | NoSuchStation of Station

  let timeToTravelBetweenStations startStation endStation train =
    if startStation = endStation then
      Error(StartAndEndStationAreTheSame startStation)
    else
      let allStops = [ train.Origin; yield! train.Stops; train.Destination ]
      let startStop = allStops |> List.tryFind (fun stop -> stop.Station = startStation)
      let endStop = allStops |> List.tryFind (fun stop -> stop.Station = endStation)

      match startStop, endStop with
      | Some startStop, Some endStop ->
        let startTime = startStop.Arrival
        let endTime = endStop.Arrival

        if startTime > endTime then
          Error(InvalidOrderOfStations(startStation, endStation))
        else
          Ok(endTime - startTime)
      | None, _ -> Error(NoSuchStation startStation)
      | _, None -> Error(NoSuchStation endStation)

  let carriagesWithFeature feature train =
    train.Carriages
    |> List.filter (fun carriage -> carriage.Features.Contains feature)

let exampleTrain =
  { Id = TrainId "ABC123"
    Origin =
      { Station = Station "London St Pancras"
        Arrival = TimeOnly(9, 0) }
    Stops =
      [ { Station = Station "Ashford"
          Arrival = TimeOnly(10, 0) }
        { Station = Station "Lille"
          Arrival = TimeOnly(12, 0) } ]
    Destination =
      { Station = Station "Paris Nord"
        Arrival = TimeOnly(13, 15) }
    DriverChange = Some(Station "Ashford")
    Carriages =
      [ { Id = CarriageId 1
          Type = Passenger First
          NumberOfSeats = 45
          Features = Set [ Wifi; Quiet; Washroom ] }
        { Id = CarriageId 2
          Type = Passenger Second
          NumberOfSeats = 65
          Features = Set [ Washroom ] }
        { Id = CarriageId 3
          Type = Buffet {| HotFood = true; ColdFood = true |}
          NumberOfSeats = 12
          Features = Set [ Wifi ] } ] }

let exampleTrainSeats = TrainFunctions.numberOfSeats exampleTrain
let wifiCarriages = TrainFunctions.carriagesWithFeature Wifi

let londonParisNord =
  TrainFunctions.timeToTravelBetweenStations (Station "London St Pancras") (Station "Paris Nord") exampleTrain

let londonParis =
  TrainFunctions.timeToTravelBetweenStations (Station "London St Pancras") (Station "Paris") exampleTrain

let parisLondon =
  TrainFunctions.timeToTravelBetweenStations (Station "Paris Nord") (Station "London St Pancras") exampleTrain

let londonLondon =
  TrainFunctions.timeToTravelBetweenStations (Station "London St Pancras") (Station "London St Pancras") exampleTrain
