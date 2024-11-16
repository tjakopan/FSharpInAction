#r "nuget: Plotly.NET, 5.1.0"
#r "nuget: FSharp.Data, 6.4.0"

open FSharp.Data
open Plotly.NET

type LondonBoroughs = HtmlProvider<const(__SOURCE_DIRECTORY__ + "/List of London boroughs - Wikipedia.html")>

let boroughs =
  LondonBoroughs.GetSample().Tables.``List of boroughs and local authorities``

let density =
  boroughs.Rows
  |> Array.map (fun row -> row.Borough, row.``Population (2019 est)`` / row.``Area (sq mi)``)

density |> Array.sortBy snd |> Chart.Column |> Chart.show
