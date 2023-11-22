using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using static Hw10.ErrorMessages.MathErrorMessager;

namespace Hw10.Services.ExpressionParser
{
    public class ExpressionParserService : IExpressionParserService
    {
        public const string Plus = "+";
        public const string Minus = "-";
        public const string Multiply = "*";
        public const string Divide = "/";
        public const string OpenBracket = "(";
        public const string CloseBracket = ")";
        public const string UnaryMinus = "_";

        public Expression ConstructExpression(string? expression)
        {
            expression = FormatIntoCorrectExpressionString(expression);

            if (!CheckExpressionString(expression, out var errorMessage))
            {
                throw new Exception(errorMessage);
            }

            var polish = GetExpressionInPolishNotation(expression!);
            var expr = CalculatePostfix(polish);

            return expr;
        }

        //Using Shunting-yard algorithm
        private Queue<string> GetExpressionInPolishNotation(string expression)
        {
            var resultQueue = new Queue<string>();

            var operators = new List<string>() { Plus, Minus, Multiply, Divide, OpenBracket, UnaryMinus };

            var tokens = expression.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            ReplaceSingleWithUnaryMinuses(tokens);

            var operatorStack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (operators.Contains(token))
                {
                    while (operatorStack.Count > 0 && HasHigherOrEqualPriority(operatorStack.Peek(), token))
                    {
                        resultQueue.Enqueue(operatorStack.Pop());
                    }

                    operatorStack.Push(token);
                }

                else if (token == CloseBracket)
                {
                    var lastOper = operatorStack.Pop();

                    while (lastOper != OpenBracket)
                    {
                        resultQueue.Enqueue(lastOper);
                        lastOper = operatorStack.Pop();
                    }
                }

                else if (double.TryParse(token, out _))
                {
                    resultQueue.Enqueue(token);
                }
            }

            while (operatorStack.Count > 0)
            {
                resultQueue.Enqueue(operatorStack.Pop());
            }

            return resultQueue;

            bool HasHigherOrEqualPriority(string op1, string op2)
            {
                var higherPriorityOpers = new List<string>() { Multiply, Divide, UnaryMinus };
                var lowerPriorityOpers = new List<string>() { Plus, Minus };

                return higherPriorityOpers.Contains(op1) && higherPriorityOpers.Contains(op2)
                    || lowerPriorityOpers.Contains(op1) && lowerPriorityOpers.Contains(op2)
                    || higherPriorityOpers.Contains(op1) && lowerPriorityOpers.Contains(op2);
            }
        }

        //Postfix Calculator algorithm
        [ExcludeFromCodeCoverage]
        private Expression CalculatePostfix(Queue<string> postfixQueue)
        {
            var exprStack = new Stack<Expression>();

            while (postfixQueue.Count > 0)
            {
                var token = postfixQueue.Dequeue();

                if (double.TryParse(token, out var number))
                {
                    exprStack.Push(Expression.Constant(number, typeof(double)));
                }

                else
                {
                    Expression expression;
                    switch (token)
                    {
                        case (Plus):
                            expression = Expression.Add(exprStack.Pop(), exprStack.Pop());
                            break;

                        case (Minus):
                            var subtract = exprStack.Pop();
                            var subtracted = exprStack.Pop();
                            expression = Expression.Subtract(subtracted, subtract);
                            break;

                        case (UnaryMinus):
                            var unar = exprStack.Pop();
                            expression = Expression.Subtract(Expression.Constant(0.0, typeof(double)), unar);
                            break;

                        case (Multiply):
                            expression = Expression.Multiply(exprStack.Pop(), exprStack.Pop());
                            break;

                        case (Divide):
                            var rightDivide = exprStack.Pop();
                            var leftDivide = exprStack.Pop();

                            expression = Expression.Divide(leftDivide, rightDivide);
                            break;

                        default:
                            throw new Exception("Unexisting token");
                    };

                    exprStack.Push(expression);
                }
            }

            return exprStack.Pop();
        }

        private bool CheckExpressionString(string? expression, out string errorMessage)
        {
            if (string.IsNullOrEmpty(expression))
            {
                errorMessage = EmptyString;
                return false;
            }

            var openBracketsStack = new Stack<string>();

            var symbols = expression.Split(' ');

            var operators = new List<string>() { Plus, Minus, Multiply, Divide };

            var previousSymb = string.Empty;

            foreach (var symb in symbols)
            {
                if (double.TryParse(symb, out var num))
                {
                    if (previousSymb == Divide && num == 0)
                    {
                        errorMessage = DivisionByZero;
                        return false;
                    }
                }

                else if (symb == OpenBracket)
                {
                    openBracketsStack.Push(symb);
                }

                else if (symb == CloseBracket)
                {
                    if (operators.Contains(previousSymb))
                    {
                        errorMessage = OperationBeforeParenthesisMessage(previousSymb);
                        return false;
                    }

                    if (!openBracketsStack.TryPop(out _))
                    {
                        errorMessage = IncorrectBracketsNumber;
                        return false;
                    }
                }

                else if (operators.Contains(symb))
                {
                    if (operators.Contains(previousSymb))
                    {
                        errorMessage = TwoOperationInRowMessage(previousSymb, symb);
                        return false;
                    }

                    if (previousSymb == string.Empty)
                    {
                        errorMessage = StartingWithOperation;
                        return false;
                    }

                    if (previousSymb == OpenBracket && symb != Minus)
                    {
                        errorMessage = InvalidOperatorAfterParenthesisMessage(symb);
                        return false;
                    }
                }

                else
                {
                    if (char.TryParse(symb, out var chr))
                    {
                        errorMessage = UnknownCharacterMessage(chr);
                        return false;
                    }

                    errorMessage = NotNumberMessage(symb);

                    return false;
                }

                previousSymb = symb;
            }

            if (openBracketsStack.Count > 0)
            {
                errorMessage = IncorrectBracketsNumber;
                return false;
            }

            if (operators.Contains(previousSymb))
            {
                errorMessage = EndingWithOperation;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        private string? FormatIntoCorrectExpressionString(string? expression)
        {
            return expression?.Replace("(", " ( ").Replace(")", " ) ").
                Replace("/", " / ").Replace("+", " + ").Replace("-", " - ").Replace("*", " * ").
                Replace("   ", " ").Replace("  ", " ").Trim();
        }

        [ExcludeFromCodeCoverage]
        private static void ReplaceSingleWithUnaryMinuses(string[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == Minus && (i == 0 || tokens[i - 1] == OpenBracket))
                {
                    tokens[i] = UnaryMinus;
                }
            }
        }
    }
}
