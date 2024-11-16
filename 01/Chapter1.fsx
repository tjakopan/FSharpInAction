type AnswerAndDate = { Answer: int; Date: System.DateTime }

let addTenThenDouble theNumber =
  let answer = (theNumber + 10) * 2

  { Answer = answer
    Date = System.DateTime.UtcNow }
