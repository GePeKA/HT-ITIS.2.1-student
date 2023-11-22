using Hw10.DbModels;
using Hw10.Dto;
using Microsoft.EntityFrameworkCore;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var cachedExpression = await _dbContext.SolvingExpressions.FirstOrDefaultAsync(x => x.Expression == expression);

		CalculationMathExpressionResultDto? result = null;

		if (cachedExpression == null) 
		{ 
			result = await _simpleCalculator.CalculateMathExpressionAsync(expression);
			
			if (result.IsSuccess)
			{
				await _dbContext.SolvingExpressions.AddAsync(new SolvingExpression()
				{
					Expression = expression!,
					Result = result.Result
				});
				await _dbContext.SaveChangesAsync();
			}
		}
		else
		{
			await Task.Delay(1000);
			result = new CalculationMathExpressionResultDto(cachedExpression.Result);
		}

		return result;
	}
}