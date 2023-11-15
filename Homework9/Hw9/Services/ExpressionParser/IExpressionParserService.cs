using System.Linq.Expressions;

namespace Hw9.Services.ExpressionParser
{
    public interface IExpressionParserService
    {
        public string? FormatIntoCorrectExpressionString(string? expression);

        public bool CheckExpressionString(string? expression, out string errorMessage);

        public Expression MakeExpression(string expression);
    }
}
