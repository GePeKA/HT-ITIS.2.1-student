open System
open Hw5.Parser
open Hw5.Calculator

let arguments = [|"15"; "/"; "5"|]
let parsed = parseCalcArguments arguments

let result = 
    match parsed with
    | Ok resultOk ->
        match resultOk with
        | val1, operation, val2 -> calculate val1 operation val2 |> string
    | Error e -> "Parse was not Successful"

printf "%s" result