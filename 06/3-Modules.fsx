module FsInAction.MyMaths

let add x y = x + y
let subtract x y = x - y

module Complicated =
  let ten = 10
  let addTogetherThenSubtractTen x y = add x y |> subtract ten
