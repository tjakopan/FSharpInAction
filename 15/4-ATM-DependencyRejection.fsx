open System
open System.IO
open System.Text

type Customer = { Name: string; Balance: decimal }

let describeCustomer customer =
  let output = StringBuilder()
  output.AppendLine $"Welcome, {customer.Name}!" |> ignore

  if customer.Balance < 0M then
    output.AppendLine $"You owe ${Math.Abs customer.Balance}." |> ignore
  else
    output.AppendLine $"You have a positive balance of {customer.Balance}."
    |> ignore

  output.ToString()

module Printers =
  let console = printfn "%s"
  let file name text = File.AppendAllText(name, text)

let customer =
  { Name = "Tomislav Jakopanec"
    Balance = 100M }

let describeToConsole customer =
  customer |> describeCustomer |> Printers.console

let describeToFile name customer =
  customer |> describeCustomer |> Printers.file name

customer |> describeToConsole
customer |> describeToFile "file.txt"
