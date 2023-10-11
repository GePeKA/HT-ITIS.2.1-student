module Hw5.Parser

open System
open System.Globalization
open Hw5.Calculator
open Hw5.MaybeBuilder

// тут убрал :Result<'a,'b>
let isArgLengthSupported (args:string[]) =
    match Array.length args with
    |3 -> Ok args
    |_ -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    |CalculatorOperation.Undefined -> Error Message.WrongArgFormatOperation
    |_ -> Ok (arg1, operation, arg2) 

let parseArgs (args: string[]): Result<('a * CalculatorOperation * 'b), Message> =
    let(parse1Success, val1) = Decimal.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture)
    let(parse2Success, val2) = Decimal.TryParse(args[2], NumberStyles.Float, CultureInfo.InvariantCulture)
    
    let operation =
        match args[1] with
        |Calculator.plus -> CalculatorOperation.Plus
        |Calculator.minus -> CalculatorOperation.Minus
        |Calculator.multiply -> CalculatorOperation.Multiply
        |Calculator.divide -> CalculatorOperation.Divide
        |_ -> CalculatorOperation.Undefined
        
    match (parse1Success, parse2Success) with
    |(true, true) -> Ok (val1, operation, val2)
    |_ -> Error Message.WrongArgFormat

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    let potentialZero = arg2 |> float
    match (operation, potentialZero) with
    |(CalculatorOperation.Divide, 0.0) -> Error Message.DivideByZero
    | _ -> Ok (arg1, operation, arg2)
    
//тут тоже
let parseCalcArguments (args: string[]) =
    maybe
        {
        let! a = args |> isArgLengthSupported
        let! b = a |> parseArgs
        let! c = b |> isOperationSupported
        let! d = c |> isDividingByZero
        return d
        }