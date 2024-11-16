open System
open ``2-Results``

let calc (a: int) (b: int) (c: int) : int = a + b * c

let tryParseNumber (numberAsString: string) =
  match Int32.TryParse numberAsString with
  | true, number -> Some number
  | false, _ -> None

let calcResult =
  match tryParseNumber "1", tryParseNumber "2", tryParseNumber "3" with
  | Some firstNumber, Some secondNumber, Some thirdNumber -> Some(calc firstNumber secondNumber thirdNumber)
  | _ -> None

#r "nuget:FsToolkit.ErrorHandling, 4.18.0"
open FsToolkit.ErrorHandling

let myMaybeData =
  option {
    let! numberOne = tryParseNumber "1"
    let! numberTwo = tryParseNumber "2"
    let! numberThree = tryParseNumber "3"
    return calc numberOne numberTwo numberThree
  }

#load "2-Results.fsx"

let validateCustomerCE rawCustomer =
  validation {
    let! customerId = Validation.validateCustomerId rawCustomer.CustomerId
    and! country = Validation.validateCountry rawCustomer.Country
    and! name = Validation.validateName rawCustomer.Name

    return
      { Id = customerId
        Name = name
        Address =
          {| Street = Street rawCustomer.Street
             City = City rawCustomer.City
             Country = country |}
        Balance = AccountBalance rawCustomer.AccountBalance }
  }
