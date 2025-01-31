﻿module Hw6.Parser

open Hw6.Calculator
open Hw6.MaybeBuilder
open System
open System.Globalization

[<Literal>] 
let DivideByZero = "DivideByZero"

let isOperationSupported (arg1, operation, arg2) =
    match operation with
    | Plus | Minus | Multiply | Divide -> Ok (arg1, operation, arg2)
    | _ -> Error $"Could not parse value '{operation}'"

let parseArgs (value1:string, operation:string, value2:string) =
    let(parse1Success, val1) = Decimal.TryParse(value1, NumberStyles.Float, CultureInfo.InvariantCulture)
    let(parse2Success, val2) = Decimal.TryParse(value2, NumberStyles.Float, CultureInfo.InvariantCulture)
        
    match (parse1Success, parse2Success) with
    | (true, true) -> Ok (val1, operation, val2)
    | (true, false) -> Error $"Could not parse value '{value2}'"
    | _ -> Error $"Could not parse value '{value1}'"

let isDividingByZero (arg1, operation, arg2) =
    match (operation, arg2) with
    | (Divide, 0M) -> Error DivideByZero
    | _ -> Ok (arg1, operation, arg2)

let parseCalcArguments (val1:string) (operation:string) (val2:string) =
    maybe
        {
        let! parsed = (val1, operation, val2) |> parseArgs
        let! operationSupported = parsed |> isOperationSupported
        let! checkDivideByZero = operationSupported |> isDividingByZero
        return checkDivideByZero
        }