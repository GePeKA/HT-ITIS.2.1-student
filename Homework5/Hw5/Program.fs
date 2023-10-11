open System
open Hw5.Parser
open Hw5.Calculator

let arguments = [|"15"; "/"; "5"|]
let parsed = parseCalcArguments arguments

let result = 
    match parsed with
    |Ok resultOk -> 
        match resultOk with
        |val1, operation, val2 -> calculate val1 operation val2
    |Error e -> raise(InvalidOperationException("Parse was not successful"))

printf "%f" result