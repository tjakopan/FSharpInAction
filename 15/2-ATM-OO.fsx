open System
open System.IO

type Customer = { Name: string; Balance: decimal }

type IPrinter =
  abstract Print: string -> unit

module IPrinters =
  let toConsole =
    { new IPrinter with
        member this.Print(text) = printfn $"{text}" }

  let toFile filename =
    { new IPrinter with
        member this.Print(text) = File.AppendAllText(filename, $"{text}") }

type Atm(customer: Customer) =
  member this.DescribeCustomer(printer: IPrinter) =
    printer.Print $"Welcome, {customer.Name}!"

    if customer.Balance < 0M then
      printer.Print $"You owe ${Math.Abs customer.Balance}."
    else
      printer.Print $"You have a positive balance of {customer.Balance}."
