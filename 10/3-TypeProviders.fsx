#r "nuget: FSharp.Data, 6.4.0"
open FSharp.Data

[<Literal>]
let sampleJson =
  """{
"Brand": "Ibanez",
"Strings": "7",
"Pickups": ["H", "S", "H"]
}
"""

type GuitarJson = JsonProvider<sampleJson>
// type GuitarJson = JsonProvider<const(__SOURCE_DIRECTORY__ + "/sample-guitars.json")>
let sample = GuitarJson.GetSample()
let description = $"{sample.Brand} has {sample.Strings} strings."

type ManyGuitarsOverHttp =
  JsonProvider<"https://raw.githubusercontent.com/isaacabraham/fsharp-in-action/main/sample-guitars.json">

let inventory = ManyGuitarsOverHttp.GetSamples()
$"You have {inventory.Length} guitars in stock."

let fullInventory =
  ManyGuitarsOverHttp.Load(__SOURCE_DIRECTORY__ + "/full-inventory.json")

fullInventory[0]

type LondonBoroughs = HtmlProvider<const(__SOURCE_DIRECTORY__ + "/List of London boroughs - Wikipedia.html")>

let boroughs =
  LondonBoroughs.GetSample().Tables.``List of boroughs and local authorities``

boroughs.Rows |> Array.map (_.Borough)
