using System.Linq.Expressions;

namespace Hw13.Calculator.Services.ExpressionCalculator
{
    public interface IExpressionCalculatorService
    {
        public Task<double> CalculateExpressionAsync(Expression expression);
    }
}
