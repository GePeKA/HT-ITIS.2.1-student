using System.Diagnostics.CodeAnalysis;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<string> Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        var calculatorHandler = new CalculatorHandler(calculator);

        return calculatorHandler.Solve(val1, operation, val2);
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return View();
    }
}