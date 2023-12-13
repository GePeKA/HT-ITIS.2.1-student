using System.Linq.Expressions;

namespace Hw13.Calculator.Services.ExpressionParser
{
    public interface IExpressionParserService
    {
        public Expression ConstructExpression(string expression);
    }
}
