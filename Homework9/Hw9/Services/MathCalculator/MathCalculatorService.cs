using Hw9.Dto;
using Hw9.Services.ExpressionParser;
using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public IExpressionParserService ExpressionParser { get; set; }

    public MathCalculatorService(IExpressionParserService expressionParserService)
    {
        ExpressionParser = expressionParserService;
    }

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        expression = ExpressionParser.FormatIntoCorrectExpressionString(expression);

        if (!ExpressionParser.CheckExpressionString(expression, out var errorMessage))
        {
            return new CalculationMathExpressionResultDto(errorMessage);
        }

        var expr = ExpressionParser.MakeExpression(expression!);

        if (expr.NodeType == ExpressionType.Throw)
        {
            var exception = (((expr as UnaryExpression)!.Operand as ConstantExpression)!
                .Value as Exception)!.Message;

            return new CalculationMathExpressionResultDto(exception);
        }

        var calculateExpression = await Task.Run(() => Expression.Lambda<Func<double>>(expr).Compile());

        return new CalculationMathExpressionResultDto(calculateExpression());
    }
}