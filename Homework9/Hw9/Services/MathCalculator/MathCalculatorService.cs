using Hw9.Dto;
using Hw9.Services.ExpressionCalculator;
using Hw9.Services.ExpressionParser;
using System.Linq.Expressions;
using static Hw9.ErrorMessages.MathErrorMessager;

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
        Expression expr;

        try
        {
            expr = ExpressionParser.ConstructExpression(expression!);
        }
        catch(Exception e)
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