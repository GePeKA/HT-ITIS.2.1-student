module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) =
    Array.length args = 3

let parseOperation (arg : string) =
    match arg with
    |"+" -> CalculatorOperation.Plus
    |"-" -> CalculatorOperation.Minus
    |"*" -> CalculatorOperation.Multiply
    |"/" -> CalculatorOperation.Divide
    |_ -> CalculatorOperation.Undefined
    
let parseCalcArguments(args : string[]) =
    if not (isArgLengthSupported args) then
        ArgumentException("Incorrect argument count") |> raise
    
    let(parse1Success, val1) = Double.TryParse(args[0])
    let(parse2Success, val2) = Double.TryParse(args[2])
    if not(parse1Success && parse2Success) then
        ArgumentException("Incorrect values") |> raise

    let parsedOp = parseOperation args[1]
    if (parsedOp = CalculatorOperation.Undefined) then
        ArgumentException("Incorrect operation") |> raise

    let options = {
        arg1 = val1;
        arg2 = val2;
        operation = parsedOp;}
    options
