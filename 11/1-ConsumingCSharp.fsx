open System
open System.IO

let fileInfo = FileInfo($"{__SOURCE_DIRECTORY__}/{__SOURCE_FILE__}")
let directoryInfo = fileInfo.Directory
let files = directoryInfo.GetFiles()

open System.Linq

let scriptFiles =
  files
  |> Seq.where (fun f -> Path.GetExtension f.Name = ".fsx")
  |> Seq.map (_.Name)

let scriptFilesLinq =
  files.Where(fun f -> Path.GetExtension f.Name = ".fsx").Select(fun f -> f.Name)

let ctorSuccinct = FileInfo __SOURCE_FILE__
let ctorVerbose = new FileInfo(__SOURCE_FILE__)

type MyDisposableType() =
  interface IDisposable with
    member _.Dispose() = printfn "Disposing!"

type MyInterface =
  abstract Capitalize: string -> string
  abstract Add: int -> int

type MyImplementation() =
  interface MyInterface with
    member this.Add(var0) = var0 + 1
    member this.Capitalize(var0) = var0.ToUpper()

//let implementation = MyImplementation()
//implementation.Capitalize "test" // compiler error

let implementation: MyInterface = MyImplementation()
let text = implementation.Capitalize "test"

type MyInterfaceAsRecord =
  { Capitalize: string -> string
    Add: int -> int }

let implementation2 =
  { new MyInterface with
      member this.Add(var0) = var0 + 1
      member this.Capitalize(var0) = var0.ToUpper() }

let text2 = implementation2.Capitalize "test"

type Person =
  { Name: string
    Age: int }

  interface ICloneable with
    member this.Clone() = { Name = this.Name; Age = this.Age }

type WebFramework() =
  member this.AddAuthentication() = this
  member this.AddCors() = this
  member this.AddCaching() = this

let setUpWeb () =
  let framework = WebFramework()
  framework.AddAuthentication().AddCors().AddCaching() |> ignore
  Ok()

let parsed, value = Int32.TryParse "123"

let parseOption parser (value: string) =
  match parser value with
  | true, v -> Some v
  | false, _ -> None

let parseIntOption = parseOption Int32.TryParse
let maybeANumber = parseIntOption "123"
let maybeNotANumber = parseIntOption "123S"

#r "nuget: FsToolkit.ErrorHandling, 4.18.0"
let xInt: int option = FsToolkit.ErrorHandling.Option.tryParse "1"
let xBool: bool option = FsToolkit.ErrorHandling.Option.tryParse "true"
let xNotABool: bool option = FsToolkit.ErrorHandling.Option.tryParse "dfere3"

type IDisplayTime =
  abstract Display: DateTime -> string

let makeIDisplayTime implementation =
  { new IDisplayTime with
      member this.Display(var0) = implementation var0 }

let normalPrinter = makeIDisplayTime (fun date -> $"The time is now {date}!")

let shortPrinter =
  makeIDisplayTime (fun date -> $"It's {date.ToShortTimeString()}.")

normalPrinter.Display DateTime.UtcNow
shortPrinter.Display DateTime.UtcNow

module File =
  let append path text =
    File.AppendAllText(path, text)
    path

  File.WriteAllText("text.txt", "test")

let fileInfo2 =
  "text.txt"
  |> File.ReadAllText
  |> fun text -> text.ToUpper() |> File.append "otherfile.txt" |> FileInfo
