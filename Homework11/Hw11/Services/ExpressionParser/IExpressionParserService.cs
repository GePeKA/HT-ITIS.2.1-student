using System.Linq.Expressions;

namespace Hw11.Services.ExpressionParser
{
    public interface IExpressionParserService
    {
        public Expression ConstructExpression(string expression);
    }
}
