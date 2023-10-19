module Hw6.Calculator

[<Literal>] 
let Plus = "Plus"

[<Literal>] 
let Minus = "Minus"

[<Literal>] 
let Multiply = "Multiply"

[<Literal>] 
let Divide = "Divide"

let inline calculate value1 operation value2 =
    match operation with
    | Plus -> value1 + value2
    | Minus -> value1 - value2
    | Multiply -> value1 * value2
    | Divide -> value1 / value2