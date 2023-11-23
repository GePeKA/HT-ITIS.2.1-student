using System.Linq.Expressions;

namespace Hw10.Services.ExpressionParser
{
    public interface IExpressionParserService
    {
        public Expression ConstructExpression(string expression);
    }
}
