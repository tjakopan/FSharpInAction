type OverdraftDetails =
  { Approved: bool
    MaxAmount: decimal
    CurrentAmount: decimal }

type CustomerWithOverdraft =
  { YearsOfHistory: int
    Overdraft: OverdraftDetails
    Country: string }

let canTakeOutALoanRecursive customer =
  match customer with
  | { YearsOfHistory = 0
      Overdraft = { Approved = true } } -> true
  | { YearsOfHistory = 0 } -> false
  | { YearsOfHistory = 1
      Overdraft = { Approved = true } } -> true
  | { YearsOfHistory = 1 } -> false
  | _ -> true

let canTakeOutALoanRecursiveCountry customer =
  match customer with
  | { YearsOfHistory = 0 | 1
      Overdraft = { Approved = true }
      Country = "US" } -> true
  | { YearsOfHistory = 0 | 1 } -> false
  | _ -> true

let bindingExample customer =
  match customer with
  | { Overdraft = { Approved = true
                    CurrentAmount = amount } } ->
    printfn $"Loan approved; current overdraft is {amount}"
    true
  | { Overdraft = { Approved = false } as overdraftDetails } ->
    printfn $"Loan declined; overdraft details are {overdraftDetails}"
    true

type LoanRequest =
  { YearsOfHistory: int
    HasOverdraft: bool
    LoanRequestAmount: decimal
    IsLargeRequest: bool }

let summariseLoanRequests requests =
  match requests with
  | [] -> "No requests made!"
  | [ { IsLargeRequest = true } ] -> "Single large request!"
  | [ { IsLargeRequest = true }; { IsLargeRequest = true } ] -> "Two large requests!"
  | { IsLargeRequest = false } :: remainingItems ->
    $"Several items, the first of which is a small request. Remaining items: {remainingItems}."
  | _ :: { HasOverdraft = true } :: _ -> "Second item has an overdraft!"
  | _ -> "Anything else"

let matchOnBoolean x =
  match x with
  | true -> "Value is true!"
  | false -> "Value is false!"

let ifThenOnBoolean x =
  if x then "Value is true!" else "Value is false!"
