using Hw13.Calculator.Dto;
using System.Linq.Expressions;
using Hw13.Calculator.Services.ExpressionParser;
using Hw13.Calculator.Services.ExpressionCalculator;
using static Hw13.Calculator.ErrorMessages.MathErrorMessager;

namespace Hw13.Calculator.Services.MathCalculator;

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

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        Expression expr;

        try
        {
            expr = ExpressionParser.ConstructExpression(expression!);
        }
        catch (Exception e)
        {
            return new CalculationMathExpressionResultDto(e.Message);
        }

        if (expr is ConstantExpression constant)
        {
            return new CalculationMathExpressionResultDto((double)constant.Value!);
        }

        var result = await ExpressionCalculator.CalculateExpressionAsync(expr);

        if (double.IsFinite(result))
            return new CalculationMathExpressionResultDto(result);
        else
            return new CalculationMathExpressionResultDto(DivisionByZero);
    }
}