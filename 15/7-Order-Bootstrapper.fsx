open System
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
  let validateOrder (logMsg: LogMsg) loadCustomer (order: Order) =
    logMsg "Loading customer from DB..."
    let customer = loadCustomer order.CustomerId
    logMsg "Validated order!"
    Ok(ValidatedOrder order)

  let processOrder validateOrder dispatchOrder order =
    let validatedOrder = validateOrder order

    match validatedOrder with
    | Ok validatedOrder -> dispatchOrder validatedOrder
    | Error e -> Error e

module WireUp =
  let processOrder: Order -> Result<_, string> =
    let authToken = Environment.GetEnvironmentVariable "AUTH_TOKEN" |> AuthToken

    let connectionString =
      Environment.GetEnvironmentVariable "CONNECTION_STRING" |> SqlConnectionString

    let logger text = printfn $"{text}"

    let validateOrder =
      let loadCustomer = IO.loadCustomer connectionString
      BusinessLogic.validateOrder logger loadCustomer

    let dispatchOrder = IO.dispatchOrder authToken
    BusinessLogic.processOrder validateOrder dispatchOrder
