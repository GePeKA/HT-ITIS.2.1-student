using System.Linq.Expressions;

namespace Hw11.Services.ExpressionCalculator
{
    public class DynamicExpressionVisitor
    {
        private Dictionary<Expression, List<Expression>> _expressionsMap;

        public Dictionary<Expression, List<Expression>> ConstructExecuteBeforeMap(Expression expression)
        {
            _expressionsMap = new();
            VisitExpression(expression);
            return _expressionsMap;
        }

        private T VisitExpression<T>(T expression) where T: Expression
        {
            return Visit((dynamic)expression);
        }

        private Expression Visit(ConstantExpression constant) 
        {
            return constant;
        }

        private Expression Visit(BinaryExpression binary)
        {
            var left = VisitExpression(binary.Left);
            var right = VisitExpression(binary.Right);

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
