using Hw1;

try
{
    Parser.ParseCalcArguments(args, out var val1, out var operation, out var val2);
    // TODO: implement calculator logic
    var result = Calculator.Calculate(val1, operation, val2);
    Console.WriteLine(result);
}
catch (Exception exc)
{
    Console.WriteLine(exc.Message);
}

