open FsToolkit.ErrorHandling
open Microsoft.AspNetCore.Builder
open Giraffe
open Microsoft.AspNetCore.Http

module Db =
  let tryFindDeviceStatus deviceId =
    task {
      if deviceId < 50 then return Some "ACTIVE"
      elif deviceId < 100 then return Some "IDLE"
      else return None
    }

type DeviceStatusError =
  | NoDeviceIdSupplied
  | InvalidDeviceId of string
  | NoSuchDeviceId of int

  member this.Description =
    match this with
    | NoDeviceIdSupplied -> "No device id was provided."
    | InvalidDeviceId text -> $"'{text}' is not a valid device id."
    | NoSuchDeviceId deviceId -> $"Device id {deviceId} does not exist."

type DeviceStatusResponse = { DeviceId: int; DeviceStatus: string }

let tryGetDeviceStatus maybeDeviceId =
  taskResult {
    let! rawDeviceId = maybeDeviceId |> Result.requireSome NoDeviceIdSupplied
    let! deviceId = Option.tryParse rawDeviceId |> Result.requireSome (InvalidDeviceId rawDeviceId)
    let! deviceStatus = Db.tryFindDeviceStatus deviceId
    let! deviceStatus = deviceStatus |> Result.requireSome (NoSuchDeviceId deviceId)

    return
      { DeviceId = deviceId
        DeviceStatus = deviceStatus }
  }

let warehouseApi next (ctx: HttpContext) =
  task {
    let maybeDeviceId = ctx.TryGetQueryStringValue "deviceId"
    let! deviceStatus = tryGetDeviceStatus maybeDeviceId

    match deviceStatus with
    | Error errorCode -> return! RequestErrors.BAD_REQUEST errorCode.Description next ctx
    | Ok deviceInfo -> return! json deviceInfo next ctx
  }

let giraffeApp = GET >=> route "/device/status" >=> warehouseApi

let builder = WebApplication.CreateBuilder()
builder.Services.AddGiraffe() |> ignore
let app = builder.Build()
app.UseGiraffe giraffeApp
app.Run()
