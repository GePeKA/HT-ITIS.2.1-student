using Hw13.Calculator.Dto;

namespace Hw13.Calculator.Services;

public interface IMathCalculatorService
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}