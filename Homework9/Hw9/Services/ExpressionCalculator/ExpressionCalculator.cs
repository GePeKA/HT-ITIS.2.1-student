using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw9.Services.ExpressionCalculator
{
    public class ExpressionCalculator : IExpressionCalculator
    {
        private ConcurrentDictionary<Expression, double> _results = new();

        public async Task<double> CalculateExpressionAsync(Expression expression)
        {
            var mapper = new ExpressionMapper();

            var executeBefore = mapper.MapExpression(expression);

            var lazy = new Dictionary<Expression, Lazy<Task>>();

            foreach(var (expr, beforeList) in executeBefore)
            {
                lazy[expr] = new Lazy<Task>(async () =>
                {
                    await Task.WhenAll(beforeList.Select(b => lazy[b].Value));
                    await Task.Yield();

                    if (expr is ConstantExpression constant)
                        await CalculateConstantAsync(constant);

                    else if (expr is BinaryExpression binary)
                        await CalculateBinaryAsync(binary);
                });
            }
            await Task.WhenAll(lazy.Values.Select(l => l.Value));

            return _results[expression];
        }

        [ExcludeFromCodeCoverage]
        private async Task<double> CalculateBinaryAsync(BinaryExpression expression)
        {
            await Task.Delay(1000);

            var left = _results[expression.Left];
            var right = _results[expression.Right];

            var result = expression.NodeType switch
            {
                ExpressionType.Add => left + right,
                ExpressionType.Subtract => left - right,
                ExpressionType.Divide => left / right,
                ExpressionType.Multiply => left * right,
                _ => throw new InvalidOperationException()
            };

            _results[expression] = result;
            return _results[expression];
        }

        private async Task<double> CalculateConstantAsync(ConstantExpression constant)
        {
            return await Task.Run(() =>
            {
                _results[constant] = (double)constant.Value!;
                return _results[constant];
            });
        }
    }
}
