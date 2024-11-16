let value = System.ConsoleModifiers.Alt

let description =
  match value with
  | System.ConsoleModifiers.Alt -> "Alt was pressed."
  | System.ConsoleModifiers.Control -> "You hit control!"
  | System.ConsoleModifiers.Shift -> "Shift held down..."
  | _ -> "Some modifier was pressed"

let customerDetails = 2, true

let canTakeOutALoan =
  match customerDetails with
  | 0, true -> false
  | 0, false -> false
  | 1, true -> false
  | 1, false -> true
  | 2, true -> true
  | 2, false -> true
  | _, true -> true
  | _, false -> true

let canTakeOutALoanV2 =
  match customerDetails with
  | 0, _ -> false
  | 1, true -> false
  | 1, false -> true
  | _ -> true

let canTakeOutALoanV3 =
  match customerDetails with
  | 0, _
  | 1, true -> false
  | 1, false
  | _ -> true

let canTakeOutALoanV4 =
  match customerDetails with
  | 0, _
  | 1, true -> false
  | _ -> true

type CustomerDetails =
  { YearsOfHistory: int
    HasOverdraft: bool }

let customerDetailsRecord =
  { YearsOfHistory = 2
    HasOverdraft = true }

let canTakeOutALoanRecord =
  match customerDetailsRecord with
  | { YearsOfHistory = 0 } -> false
  | { YearsOfHistory = 1
      HasOverdraft = true } -> false
  | { YearsOfHistory = 1
      HasOverdraft = false } -> true
  | _ -> true

let canTakeOutALoanRecordFn customer =
  match customer with
  | { YearsOfHistory = 0 } -> false
  | { YearsOfHistory = 1
      HasOverdraft = true } -> false
  | { YearsOfHistory = 1
      HasOverdraft = false } -> true
  | _ -> true

type CustomerDetailsOverdraft =
  { YearsOfHistory: int
    HasOverdraft: bool
    Overdraft: int }

let customerDetailsWithOverdraftRecord =
  { YearsOfHistory = 2
    HasOverdraft = true
    Overdraft = 500 }

let canTakeOutALoanWithGuard =
  match customerDetailsWithOverdraftRecord with
  | { YearsOfHistory = 0 } -> false
  | { YearsOfHistory = 1
      HasOverdraft = true } when customerDetailsWithOverdraftRecord.Overdraft > 500 -> false
  | { YearsOfHistory = 1 } -> true
  | _ -> true

let canTakeOutALoanWithFunctionGuard =
  let maxOverdraft yearsOfHistory = yearsOfHistory * 250

  match customerDetailsWithOverdraftRecord with
  | { YearsOfHistory = 0 } -> false
  | { YearsOfHistory = 1 | 2 as years
      HasOverdraft = true } when customerDetailsWithOverdraftRecord.Overdraft > maxOverdraft years -> false
  | { YearsOfHistory = 1 | 2 } -> true
  | _ -> true
