#r "nuget: FsToolkit.ErrorHandling, 4.18.0"

open System
open FsToolkit.ErrorHandling
open Microsoft.FSharp.Core

type AuthToken = AuthToken of string
type Order = { CustomerId: int }
type ValidatedOrder = ValidatedOrder of Order
type SqlConnectionString = SqlConnectionString of string
type Customer = { CustomerId: int; Name: string }
type LogMsg = string -> unit

module IO =
  let dispatchOrder (authToken: AuthToken) (order: ValidatedOrder) =
    Ok
      {| DispatchDate = DateTime.Today.AddDays 2.
         Status = "Dispatched" |}

  let loadCustomer (connectionString: SqlConnectionString) customerId =
    Ok
      { CustomerId = customerId
        Name = "John Doe" }

module BusinessLogic =
  let validateOrder (order: Order) (customer: Customer) =
    [ "Checking customer balance..."; "Validated order!" ], Ok(ValidatedOrder order)

  let processOrder (order: Order) =
    result {
      let authToken = Environment.GetEnvironmentVariable "AUTH_TOKEN" |> AuthToken

      let connectionString =
        Environment.GetEnvironmentVariable "CONNECTION_STRING" |> SqlConnectionString

      let logger text = printfn $"{text}"

      logger "Loading customer from DB..."
      let! customer = IO.loadCustomer connectionString order.CustomerId
      let messages, validationResult = validateOrder order customer

      for message in messages do
        logger message

      let! validatedOrder = validationResult
      return! IO.dispatchOrder authToken validatedOrder
    }
