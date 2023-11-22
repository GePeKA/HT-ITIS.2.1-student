using System.Linq.Expressions;

namespace Hw10.Services.ExpressionCalculator
{
    public class ExpressionMapper : ExpressionVisitor
    {
        private readonly Dictionary<Expression, List<Expression>> _expressionsMap = new();

        public Dictionary<Expression, List<Expression>> ConstructExecuteBeforeMap(Expression expression)
        {
            Visit(expression);
            return _expressionsMap;
        }

        protected override Expression VisitBinary(BinaryExpression binary)
        {
            var left = Visit(binary.Left);
            var right = Visit(binary.Right);

            var executeBeforeList = new List<Expression>();

            if (left is BinaryExpression leftBinary)
                executeBeforeList.Add(leftBinary);

            if (right is BinaryExpression rightBinary)
                executeBeforeList.Add(rightBinary);

            _expressionsMap.Add(binary, executeBeforeList);

            return binary;
        }
    }
}
