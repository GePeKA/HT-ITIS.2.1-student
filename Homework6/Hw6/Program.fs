module Hw6.App

open System
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.AspNetCore.Http
open System.Globalization

let inline calculate val1 oper val2 = 
    match oper with
    | "Plus" -> Ok ((val1 + val2) |> string)
    | "Minus" -> Ok ((val1 - val2) |> string)
    | "Divide" -> 
        match val2 |> double with
        | 0.0 -> Ok "DivideByZero"
        | _ -> Ok ((val1 / val2) |> string)
    | "Multiply" -> Ok ((val1 * val2) |> string)
    | _ -> Error $"Could not parse value '{oper}'"

let calculateFromCtx (ctx : HttpContext) =
    let query1 = ctx.Request.Query["value1"] |> string
    let query2 = ctx.Request.Query["value2"] |> string
    
    let (success1, val1) = Decimal.TryParse(query1, NumberStyles.Float, CultureInfo.InvariantCulture)
    let oper = ctx.Request.Query["operation"] |> string
    let (success2, val2) = Decimal.TryParse(query2, NumberStyles.Float, CultureInfo.InvariantCulture)

    match (success1, success2) with
    | (true, true) -> calculate val1 oper val2
    | (true, false) -> Error $"Could not parse value '{query2}'"
    | _ -> Error $"Could not parse value '{query1}'"

let calculatorHandler: HttpHandler =
    fun next ctx ->
        let result: Result<string, string> = calculateFromCtx ctx

        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error -> (setStatusCode 400 >=> text ("Error occured: " + error)) next ctx

let webApp =
    choose [
        GET >=> choose [
             route "/" >=> text "Use /calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>"
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found" 
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseGiraffe webApp
        
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0