module Hw6.Calculator

[<Literal>] 
let Plus = "Plus"

[<Literal>] 
let Minus = "Minus"

[<Literal>] 
let Multiply = "Multiply"

[<Literal>] 
let Divide = "Divide"

let calculate (value1:decimal) (operation:string) (value2:decimal) =
    match operation with
    | Plus -> value1 + value2
    | Minus -> value1 - value2
    | Multiply -> value1 * value2
    | Divide -> value1 / value2