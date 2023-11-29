using Hw11.Services.ExpressionCalculator;
using Hw11.Services.ExpressionParser;
using System.Linq.Expressions;
using static Hw11.ErrorMessages.MathErrorMessager;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public IExpressionParserService ExpressionParser { get; }
    public IExpressionCalculatorService ExpressionCalculator { get; }

    public MathCalculatorService(IExpressionParserService expressionParserService,
        IExpressionCalculatorService expressionCalculator)
    {
        ExpressionParser = expressionParserService;
        ExpressionCalculator = expressionCalculator;
    }

    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        Expression expr;

        expr = ExpressionParser.ConstructExpression(expression!);

        if (expr is ConstantExpression constant)
            return (double)constant.Value!;

        var result = await ExpressionCalculator.CalculateExpressionAsync(expr);

        if (double.IsFinite(result))
            return result;
        else
            throw new DivideByZeroException(DivisionByZero);
    }
}