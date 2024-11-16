open System

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
  let validateOrder (logMsg: LogMsg) connectionString (order: Order) =
    logMsg "Loading customer from DB..."
    let customer = IO.loadCustomer connectionString order.CustomerId
    logMsg "Validated order!"
    Ok(ValidatedOrder order)

  let processOrder logMsg authToken connectionString order =
    let validatedOrder = validateOrder logMsg connectionString order

    match validatedOrder with
    | Ok validatedOrder -> IO.dispatchOrder authToken validatedOrder
    | Error e -> Error e
