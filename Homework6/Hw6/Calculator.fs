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
    | Plus -> Ok (value1 + value2)
    | Minus -> Ok (value1 - value2)
    | Multiply -> Ok (value1 * value2)
    | Divide -> Ok (value1 / value2)
    | _ -> Error $"Wrong Operation: {operation}"