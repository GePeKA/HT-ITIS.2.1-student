using System.Linq.Expressions;

namespace Hw9.Services.ExpressionParser
{
    public interface IExpressionParserService
    {
        public Expression ConstructExpression(string expression);
    }
}
