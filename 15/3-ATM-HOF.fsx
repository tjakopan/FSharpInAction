open System
open System.IO

type Customer = { Name: string; Balance: decimal }

let describeCustomer printer customer =
  printer $"Welcome, {customer.Name}!"

  if customer.Balance < 0M then
    printer $"You owe ${Math.Abs customer.Balance}."
  else
    printer $"You have a positive balance of {customer.Balance}."

module Printers =
  let console = printfn "%s"
  let file name text = File.AppendAllText(name, text)

let customer =
  { Name = "Tomislav Jakopanec"
    Balance = 100M }

customer |> describeCustomer Printers.console
customer |> describeCustomer (Printers.file "file.txt")
