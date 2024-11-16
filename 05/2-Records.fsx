open Microsoft.FSharp.Core

type Person =
  { FirstName: string
    LastName: string
    Age: int }

let isaac =
  { FirstName = "Isaac"
    LastName = "Abraham"
    Age = 42 }

let fullName = $"{isaac.FirstName} {isaac.LastName}"

type Address =
  { Line1: string
    Line2: string
    Town: string
    Postcode: string
    Country: string }

type PersonSimple =
  { Name: string * string
    Address: Address }

type PersonWithDescription =
  { Name: string
    Age: int
    Description: string }

let buildPerson forename surname age =
  { Name = $"{forename} {surname}"
    Age = 42
    Description = if age < 18 then "child" else "adult" }

let generatePerson theAddress =
  if theAddress.Country = "UK" then
    { Name = "Isaac", "Abraham"
      Address = theAddress }
  else
    { Name = "John", "Doe"
      Address = theAddress }

let theAddress =
  { Line1 = "1 Main Street"
    Line2 = ""
    Town = "London"
    Postcode = "SW1 1AA"
    Country = "UK" }

let isaacAddr: PersonSimple =
  { Name = "Isaac", "Abraham"
    Address = theAddress }

let isaacAddrTwo =
  { PersonSimple.Name = "Isaac", "Abraham"
    Address = theAddress }

let theAddressTwo =
  { Line1 = "1st Street"
    Line2 = "Apt. 1"
    Town = "London"
    Postcode = "SW1 1AA"
    Country = "UK" }

let theAddressInDE =
  { theAddress with
      Town = "Berlin"
      Country = "DE" }

let isaacOne =
  { Name = "Isaac", "Abraham"
    Address = theAddress }

let isaacTwo =
  { Name = "Isaac", "Abraham"
    Address = theAddress }

let areTheyTheSame = (isaacOne = isaacTwo)

type Name = { Forename: string; Surname: string }
type CreditRating = CreditRating of int

type Customer =
  { Name: Name
    Address: Address
    CreditRating: CreditRating }

type Balance = Balance of decimal

type Supplier =
  { Name: Name
    Address: Address
    OutstandingBalance: Balance
    NextPaymentDate: System.DateOnly }

let company =
  {| Name = "My Company Inc."
     Town = "The Town"
     Country = "UK"
     TaxNumber = 123456 |}

let companyWithBankDetails =
  {| company with
      AccountNumber = 123
      SortCode = 456 |}

type PersonAnon =
  { Name: string * string
    Address:
      {| Line1: string
         Line2: string
         Town: string
         Country: string |} }
