using Hw9.Dto;
using Hw9.Services.ExpressionCalculator;
using Hw9.Services.ExpressionParser;
using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public IExpressionParserService ExpressionParser { get; set; }
    public IExpressionCalculator ExpressionCalculator { get; set; }

    public MathCalculatorService(IExpressionParserService expressionParserService,
        IExpressionCalculator expressionCalculator)
    {
        ExpressionParser = expressionParserService;
        ExpressionCalculator = expressionCalculator;
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
            var exceptionMessage = (((expr as UnaryExpression)!.Operand as ConstantExpression)!
                .Value as Exception)!.Message;

            return new CalculationMathExpressionResultDto(exceptionMessage);
        }

        else if (expr is ConstantExpression constant)
        {
            return new CalculationMathExpressionResultDto((double)constant.Value!);
        }

        var result = await ExpressionCalculator.CalculateExpressionAsync(expr);

        return new CalculationMathExpressionResultDto(result);
    }
}