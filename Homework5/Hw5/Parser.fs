module Hw5.Parser

open System
open System.Globalization
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]) =
    match Array.length args with
    | 3 -> Ok args
    | _ -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | CalculatorOperation.Undefined -> Error Message.WrongArgFormatOperation
    | _ -> Ok (arg1, operation, arg2) 

let parseArgs (args: string[]): Result<('a * CalculatorOperation * 'b), Message> =
    let(parse1Success, val1) = Decimal.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture)
    let(parse2Success, val2) = Decimal.TryParse(args[2], NumberStyles.Float, CultureInfo.InvariantCulture)
    
    let operation =
        match args[1] with
        | Calculator.Plus -> CalculatorOperation.Plus
        | Calculator.Minus -> CalculatorOperation.Minus
        | Calculator.Multiply -> CalculatorOperation.Multiply
        | Calculator.Divide -> CalculatorOperation.Divide
        | _ -> CalculatorOperation.Undefined
        
    match (parse1Success, parse2Success) with
    | (true, true) -> Ok (val1, operation, val2)
    | _ -> Error Message.WrongArgFormat

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    let potentialZero = arg2 |> float
    match (operation, potentialZero) with
    | (CalculatorOperation.Divide, 0.0) -> Error Message.DivideByZero
    | _ -> Ok (arg1, operation, arg2)

let parseCalcArguments (args: string[]) =
    maybe
        {
        let! argLengthSupported = args |> isArgLengthSupported
        let! parsed = argLengthSupported |> parseArgs
        let! operationSupported = parsed |> isOperationSupported
        let! checkDivideByZero = operationSupported |> isDividingByZero
        return checkDivideByZero
        }