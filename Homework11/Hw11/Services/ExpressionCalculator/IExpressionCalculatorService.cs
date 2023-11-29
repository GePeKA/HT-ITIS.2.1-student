using System.Linq.Expressions;

namespace Hw11.Services.ExpressionCalculator
{
    public interface IExpressionCalculatorService
    {
        public Task<double> CalculateExpressionAsync(Expression expression);
    }
}
