open System
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Core

type RawCustomer =
  { CustomerId: string
    Name: string
    Street: string
    City: string
    Country: string
    AccountBalance: decimal }

type CustomerId = CustomerId of int
type Name = Name of string
type Street = Street of string
type City = City of string

type Country =
  | Domestic
  | Foreign of string

type AccountBalance = AccountBalance of decimal

type Customer =
  { Id: CustomerId
    Name: Name
    Address:
      {| Street: Street
         City: City
         Country: Country |}
    Balance: AccountBalance }

let validateCustomer (rawCustomer: RawCustomer) =
  let customerId =
    if rawCustomer.CustomerId.StartsWith "C" then
      Ok(CustomerId(int rawCustomer.CustomerId[1..]))
    else
      Error $"Invalid customer id '{rawCustomer.CustomerId}'."

  let country =
    match rawCustomer.Country with
    | "" -> Error "No country supplied"
    | "USA" -> Ok Domestic
    | other -> Ok(Foreign(other))

  match customerId, country with
  | Ok customerId, Ok country ->
    Ok
      { Id = customerId
        Name = Name rawCustomer.Name
        Address =
          {| Street = Street rawCustomer.Street
             City = City rawCustomer.City
             Country = country |}
        Balance = AccountBalance rawCustomer.AccountBalance }
  | Error err, _
  | _, Error err -> Error err

let validateCustomerMultipleErrors (rawCustomer: RawCustomer) =
  let customerId =
    if rawCustomer.CustomerId.StartsWith "C" then
      Ok(CustomerId(int rawCustomer.CustomerId[1..]))
    else
      Error $"Invalid customer id '{rawCustomer.CustomerId}'."

  let country =
    match rawCustomer.Country with
    | "" -> Error "No country supplied"
    | "USA" -> Ok Domestic
    | other -> Ok(Foreign(other))

  match customerId, country with
  | Ok customerId, Ok country ->
    Ok
      { Id = customerId
        Name = Name rawCustomer.Name
        Address =
          {| Street = Street rawCustomer.Street
             City = City rawCustomer.City
             Country = country |}
        Balance = AccountBalance rawCustomer.AccountBalance }
  | customerId, country ->
    Error
      [ match customerId with
        | Ok _ -> ()
        | Error err -> err
        match country with
        | Ok _ -> ()
        | Error err -> err ]

type CustomerValidationError =
  | InvalidCustomerId of string
  | InvalidCountry of string

let validateCustomerStrongErrors (rawCustomer: RawCustomer) =
  let customerId =
    if rawCustomer.CustomerId.StartsWith "C" then
      Ok(CustomerId(int rawCustomer.CustomerId[1..]))
    else
      Error(InvalidCustomerId $"Invalid customer id '{rawCustomer.CustomerId}'.")

  let country =
    match rawCustomer.Country with
    | "" -> Error(InvalidCountry "No country supplied")
    | "USA" -> Ok Domestic
    | other -> Ok(Foreign(other))

  match customerId, country with
  | Ok customerId, Ok country ->
    Ok
      { Id = customerId
        Name = Name rawCustomer.Name
        Address =
          {| Street = Street rawCustomer.Street
             City = City rawCustomer.City
             Country = country |}
        Balance = AccountBalance rawCustomer.AccountBalance }
  | customerId, country ->
    Error
      [ match customerId with
        | Ok _ -> ()
        | Error err -> err
        match country with
        | Ok _ -> ()
        | Error err -> err ]

module Validation =

  type CustomerValidationErrorRich =
    | EmptyCustomerId
    | InvalidCustomerIdFormat of string
    | NoNameSupplied
    | TooManyNameParts
    | NoCountrySupplied

  let validateCustomerId customerId =
    if String.IsNullOrWhiteSpace customerId then
      Error EmptyCustomerId
    elif customerId.StartsWith "C" then
      Ok(CustomerId(int customerId[1..]))
    else
      Error(InvalidCustomerIdFormat customerId)

  let validateCountry country =
    match country with
    | "" -> Error NoCountrySupplied
    | "USA" -> Ok Domestic
    | other -> Ok(Foreign other)

  let validateName name =
    if String.IsNullOrWhiteSpace name then
      Error NoNameSupplied
    elif name.Split ' ' |> Array.length > 2 then
      Error TooManyNameParts
    else
      Ok(Name name)

let validateCustomerFull (rawCustomer: RawCustomer) =
  let customerId = Validation.validateCustomerId rawCustomer.CustomerId
  let country = Validation.validateCountry rawCustomer.Country
  let name = Validation.validateName rawCustomer.Name

  match customerId, country, name with
  | Ok customerId, Ok country, Ok name ->
    Ok
      { Id = customerId
        Name = name
        Address =
          {| Street = Street rawCustomer.Street
             City = City rawCustomer.City
             Country = country |}
        Balance = AccountBalance rawCustomer.AccountBalance }
  | customerId, country, name ->
    Error
      [ match customerId with
        | Ok _ -> ()
        | Error err -> err
        match country with
        | Ok _ -> ()
        | Error err -> err
        match name with
        | Ok _ -> ()
        | Error err -> err ]

module ValidationNested =
  type CustomerIdError =
    | EmptyId
    | InvalidIdFormat of string

  type NameError =
    | NoNameSupplied
    | TooManyParts

  type CountryError = | NoCountrySupplied

  let validateCustomerId customerId =
    if String.IsNullOrWhiteSpace customerId then
      Error EmptyId
    elif customerId.StartsWith "C" then
      Ok(CustomerId(int customerId[1..]))
    else
      Error(InvalidIdFormat customerId)

  let validateCountry country =
    match country with
    | "" -> Error NoCountrySupplied
    | "USA" -> Ok Domestic
    | other -> Ok(Foreign other)

  let validateName name =
    if String.IsNullOrWhiteSpace name then
      Error NoNameSupplied
    elif name.Split ' ' |> Array.length > 2 then
      Error TooManyParts
    else
      Ok(Name name)

type CustomerValidationErrorRoot =
  | CustomerIdError of ValidationNested.CustomerIdError
  | NameError of ValidationNested.NameError
  | CountryError of ValidationNested.CountryError

let validateCustomerNested rawCustomer =
  let customerId = ValidationNested.validateCustomerId rawCustomer.CustomerId
  let country = ValidationNested.validateCountry rawCustomer.Country
  let name = ValidationNested.validateName rawCustomer.Name

  match customerId, country, name with
  | Ok customerId, Ok country, Ok name ->
    Ok
      { Id = customerId
        Name = name
        Address =
          {| Street = Street rawCustomer.Street
             City = City rawCustomer.City
             Country = country |}
        Balance = AccountBalance rawCustomer.AccountBalance }
  | customerId, country, name ->
    Error
      [ match customerId with
        | Ok _ -> ()
        | Error err -> CustomerIdError err
        match country with
        | Ok _ -> ()
        | Error err -> CountryError err
        match name with
        | Ok _ -> ()
        | Error err -> NameError err ]

try
  Some(1 / 0)
with ex ->
  printfn $"Error: {ex.Message}"
  None

let handleException func arg =
  try
    func arg |> Ok
  with ex ->
    Error ex

let divide (a, b) = a / b
let divideSafe = handleException divide
