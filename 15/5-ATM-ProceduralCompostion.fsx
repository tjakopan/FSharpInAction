open System
open System.IO

type Customer = { Name: string; Balance: decimal }

module Customer =
  let greet customer = $"Welcome, {customer.Name}!"

  let sendMessage customer =
    if customer.Balance < 0M then
      $"You owe ${Math.Abs customer.Balance}."
    else
      $"You have a positive balance of {customer.Balance}."

module Printers =
  let console = printfn "%s"
  let file name text = File.AppendAllText(name, text)

let customer =
  { Name = "Tomislav Jakopanec"
    Balance = 100M }

let describeToConsole customer =
  Printers.console (Customer.greet customer)
  Printers.console (Customer.sendMessage customer)

let describeToFile name customer =
  Printers.file name (Customer.greet customer)
  Printers.file name (Customer.sendMessage customer)

customer |> describeToConsole
customer |> describeToFile "file.txt"
