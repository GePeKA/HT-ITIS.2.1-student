using System.Linq.Expressions;

namespace Hw9.Services.ExpressionCalculator
{
    public class ExpressionMapper: ExpressionVisitor
    {
        private readonly Dictionary<Expression, List<Expression>> _expressionsMap = new();

        public Dictionary<Expression, List<Expression>> MapExpression(Expression expression)
        {
            Visit(expression);
            return _expressionsMap;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _expressionsMap.Add(node, new List<Expression>());

            return node;
        }

        protected override Expression VisitBinary(BinaryExpression binary)
        {
            _expressionsMap.Add(binary, new List<Expression>() { Visit(binary.Left), Visit(binary.Right) });

            return binary;
        }
    }
}
