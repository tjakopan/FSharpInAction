open System
let isEvenSecond (args: Timers.ElapsedEventArgs) = args.SignalTime.Second % 2 = 0

let printTime (args: Timers.ElapsedEventArgs) =
  printfn $"Event was raised at {args.SignalTime}"

let t = new Timers.Timer(Interval = 1000., Enabled = true)

t.Elapsed |> Event.filter isEvenSecond |> Event.add printTime

t.Start()
t.Stop()

let inputs: String seq =
  seq {
    while true do
      Console.Write "Please enter your command: "
      Console.ReadLine()
  }

let traverse results =
  let oks =
    [ for result in results do
        match result with
        | Ok x -> yield x
        | Error _ -> () ]

  let errors =
    [ for result in results do
        match result with
        | Ok _ -> ()
        | Error x -> yield x ]

  match oks, errors with
  | allOk, [] -> Ok allOk
  | _, errors -> Error errors

let allOks: Result<int list, obj list> = traverse [ Ok 1; Ok 2; Ok 3 ]
let oneError = traverse [ Ok 1; Ok 2; Error "Bad" ]
let twoErrors = traverse [ Ok 1; Error "Bad 1"; Error "Bad 2" ]

#r "nuget: FsToolkit.ErrorHandling, 4.18.0"

let twoErrorsM =
  FsToolkit.ErrorHandling.List.traverseResultM id [ Ok 1; Error "Bad 1"; Error "Bad 2" ]

let twoErrorsA =
  FsToolkit.ErrorHandling.List.traverseResultA id [ Ok 1; Error "Bad 1"; Error "Bad 2" ]

let nestedErrors =
  FsToolkit.ErrorHandling.List.traverseValidationA id [ Ok 1; Error [ "Bad 1" ]; Error [ "Bad 2"; "Bad 3" ] ]
