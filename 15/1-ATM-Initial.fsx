open System

type Customer = { Name: string; Balance: decimal }

let describeCustomer customer =
  printfn $"Welcome, {customer.Name}!"

  if customer.Balance < 0M then
    printfn $"You owe ${Math.Abs customer.Balance}."
  else
    printfn $"You have a positive balance of {customer.Balance}."
