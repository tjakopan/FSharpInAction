[<AutoOpen>]
module Domain =
  type ValidatedEmail =
    private
    | ValidatedEmail of string

    member this.Value =
      match this with
      | ValidatedEmail e -> e

    static member TryCreate(unvalidatedEmail: string) =
      if unvalidatedEmail.Contains "@" then
        Ok(ValidatedEmail unvalidatedEmail)
      else
        Error "Invalid email!"

let sendEmailSafe message (email: ValidatedEmail) = Ok "Sent message"

let sendResult =
  ValidatedEmail.TryCreate "isaac@email.com"
  |> Result.bind (sendEmailSafe "Welcome!")
