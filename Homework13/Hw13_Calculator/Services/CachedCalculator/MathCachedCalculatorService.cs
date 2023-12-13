using Hw13.Calculator.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace Hw13.Calculator.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly IMemoryCache _cache;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(IMemoryCache cache, IMathCalculatorService simpleCalculator)
	{
		_cache = cache;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		return await _cache.GetOrCreateAsync(expression, async _ =>
        {
            var res = await _simpleCalculator.CalculateMathExpressionAsync(expression);
            return res.IsSuccess
                ? new CalculationMathExpressionResultDto(res.Result)
                : new CalculationMathExpressionResultDto(res.ErrorMessage);
        });
    }
}