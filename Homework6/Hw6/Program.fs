module Hw6.App

open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Hw6.Parser
open Hw6.Calculator


let calculatorHandler: HttpHandler =
    fun next ctx ->
        let query1 = ctx.Request.Query["value1"] |> string
        let query2 = ctx.Request.Query["value2"] |> string
        let oper = ctx.Request.Query["operation"] |> string
        
        let result: Result<string, string> = 
            match (parseCalcArguments query1 oper query2) with
            | Ok(val1, op, val2) -> Ok ((calculate val1 op val2) |> string)
            | Error error -> Error error 

        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok)) next ctx
        | Error error -> (setStatusCode 400 >=> text ($"Error occured: {error}")) next ctx

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