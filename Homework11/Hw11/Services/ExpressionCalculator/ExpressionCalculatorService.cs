using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw11.Services.ExpressionCalculator
{
    public class ExpressionCalculatorService: IExpressionCalculatorService
    {
        private ConcurrentDictionary<Expression, double> _results = new();

        public async Task<double> CalculateExpressionAsync(Expression expression)
        {
            var mapper = new DynamicExpressionVisitor();

            var executeBefore = mapper.ConstructExecuteBeforeMap(expression);

            var lazy = new Dictionary<Expression, Lazy<Task>>();

            foreach (var (expr, beforeList) in executeBefore)
            {
                lazy[expr] = new Lazy<Task>(async () =>
                {
                    await Task.WhenAll(beforeList.Select(b => lazy[b].Value));
                    await Task.Yield();

                    if (expr is BinaryExpression binary)
                        await CalculateBinaryAsync(binary);
                });
            }
            await Task.WhenAll(lazy.Values.Select(l => l.Value));

            return _results[expression];
        }

        [ExcludeFromCodeCoverage]
        private async Task<double> CalculateBinaryAsync(BinaryExpression expr)
        {
            await Task.Delay(1000);

            var left = expr.Left is ConstantExpression const1 ? (double)const1.Value! : _results[expr.Left];
            var right = expr.Right is ConstantExpression const2 ? (double)const2.Value! : _results[expr.Right];

            var result = expr.NodeType switch
            {
                ExpressionType.Add => left + right,
                ExpressionType.Subtract => left - right,
                ExpressionType.Divide => left / right,
                ExpressionType.Multiply => left * right,
                _ => throw new InvalidOperationException()
            };

            _results[expr] = result;
            return result;
        }
    }
}
