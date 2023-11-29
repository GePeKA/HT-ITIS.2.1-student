using System.Linq.Expressions;

namespace Hw11.Services.ExpressionCalculator
{
    public class DynamicExpressionVisitor
    {
        private readonly Dictionary<Expression, List<Expression>> _expressionsMap = new();

        public Dictionary<Expression, List<Expression>> ConstructExecuteBeforeMap(Expression expression)
        {
            VisitExpression(expression);
            return _expressionsMap;
        }

        public T VisitExpression<T>(T expression) where T: Expression
        {
            return Visit((dynamic)expression);
        }

        public Expression Visit(ConstantExpression constant) 
        {
            return constant;
        }

        public Expression Visit(BinaryExpression binary)
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
