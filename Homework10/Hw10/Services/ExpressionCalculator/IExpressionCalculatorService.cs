using System.Linq.Expressions;

namespace Hw10.Services.ExpressionCalculator
{
    public interface IExpressionCalculatorService
    {
        public Task<double> CalculateExpressionAsync(Expression expression);
    }
}
