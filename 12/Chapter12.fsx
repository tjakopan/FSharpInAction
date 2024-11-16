open System.IO
open System.Text.Json
open System.Threading.Tasks

File.WriteAllText("foo.txt", "Hello, world")
let text = File.ReadAllText "foo.txt"
let textAsync = File.ReadAllTextAsync "foo.txt"
let theText = textAsync.Result

let writeToFile fileName data =
  File.AppendAllText(fileName, data)
  let data = File.ReadAllText fileName
  data.Length

let total = writeToFile "sample.txt" "foo"

let writeToFileAsync fileName data =
  task {
    do! File.AppendAllTextAsync(fileName, data)
    let! data = File.ReadAllTextAsync fileName
    return data.Length
  }

let filesComposed =
  task {
    let! file1 = File.ReadAllTextAsync "file1.txt"
    let! file2 = File.ReadAllTextAsync "file2.txt"
    let! file3 = File.ReadAllTextAsync "file3.txt"
    return $"{file1} {file2} {file3}"
  }

filesComposed.Result

let filesParallel =
  task {
    let! allFiles =
      [ "file1.txt"; "file2.txt"; "file3.txt" ]
      |> List.map File.ReadAllTextAsync
      |> Task.WhenAll

    return allFiles |> Array.reduce (sprintf "%s %s")
  }

filesParallel.Result

let writeToFileAsyncMix fileName data =
  printfn "1. This is happening synchronously!"
  Task.Delay(1000).Wait()
  printfn "2. Kicking off the background work!"

  let result =
    task {
      do! File.AppendAllTextAsync(fileName, data)
      do! Task.Delay(1000)
      printfn "4. This is happening asynchronously!"
      let! data = File.ReadAllTextAsync fileName
      return data.Length
    }

  printfn "3. Doing something more, now let's return the task"
  result

let theResult = writeToFileAsyncMix "sample.txt" "foo"

let writeToFileAsyncBlock fileName data =
  async {
    do! File.AppendAllTextAsync(fileName, data) |> Async.AwaitTask
    let! data = File.ReadAllTextAsync fileName |> Async.AwaitTask
    return data.Length
  }

let asyncBlockResult = writeToFileAsyncBlock "sample.txt" "foo"
asyncBlockResult |> Async.RunSynchronously

let loadCustomerFromDb customerId = {| Name = "Isaac"; Balance = 0 |}

let tryGetCustomer customerId =
  let customer = loadCustomerFromDb customerId

  if customer.Balance <= 0 then
    Error "Customer is in debt!"
  else
    Ok customer

let handleRequest (json: string) =
  let request: {| CustomerId: int |} = JsonSerializer.Deserialize json
  let response = tryGetCustomer request.CustomerId

  match response with
  | Ok c -> {| CustomerName = c.Name.ToUpper() |}
  | Error msg -> failwith $"Bad request: {msg}"

handleRequest ({| CustomerId = 1 |} |> JsonSerializer.Serialize)

let loadCustomerFromDbAsync customerId =
  task { return {| Name = "Isaac"; Balance = 0 |} }

let tryGetCustomerAsync customerId =
  task {
    let! customer = loadCustomerFromDbAsync customerId

    return
      if customer.Balance <= 0 then
        Error "Customer is in debt!"
      else
        Ok customer
  }

let handleRequestAsync (json: string) =
  task {
    let request: {| CustomerId: int |} = JsonSerializer.Deserialize json
    let! response = tryGetCustomerAsync request.CustomerId

    return
      match response with
      | Ok c -> {| CustomerName = c.Name.ToUpper() |}
      | Error msg -> failwith $"Bad request: {msg}"
  }

let responseAsync = handleRequestAsync ({| CustomerId = 1 |} |> JsonSerializer.Serialize)