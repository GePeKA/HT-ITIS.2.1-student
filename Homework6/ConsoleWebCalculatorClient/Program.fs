open System 
open System.Net.Http

printf "%s" "Enter the first value: "
let value1 = Console.ReadLine()

Console.Write("Enter the second value: ")
let value2 = Console.ReadLine()

Console.Write("Enter the operation (Plus/Minus/Multiply/Divide): ")
let operation = Console.ReadLine();

let calculateOnServer() = 
    async {
        let url = $@"https://localhost:59811/calculate?value1={value1}&operation={operation}&value2={value2}"
        use client = new HttpClient()
        let! response = client.GetAsync url |> Async.AwaitTask
        return! response.Content.ReadAsStringAsync() |> Async.AwaitTask
    }

let answer = calculateOnServer() |> Async.RunSynchronously

printfn "%s" $"\nThe answer = {answer}"