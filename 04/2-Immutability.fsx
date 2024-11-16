let drive gas distance =
  if distance = "far" then gas / 2.0
  elif distance = "medium" then gas - 10.0
  else gas - 1.0

let gas = 100.0
let firstState = drive gas "far"
let secondState = drive firstState "medium"
let thirdState = drive secondState "near"

let drive43 gas distance =
  if distance > 50 then gas / 2.0
  elif distance > 25 then gas - 10.0
  elif distance > 0 then gas - 1.0
  else gas
