open System
open Hw4.Calculator
open Hw4.Parser

let args = [|"15";"/";"5"|]

let calc = parseCalcArguments args

Console.WriteLine(calculate calc.arg1 calc.operation calc.arg2)