using System.Globalization;

namespace Hw8.Calculator
{
    public class CalculatorHandler
    {
        public ICalculator Calculator { get; set; }

        public CalculatorHandler(ICalculator calculator)
        {
            Calculator = calculator;
        }

        public string Solve(string val1, string operation, string val2)
        {
            var parseVal1Suc = double.TryParse(val1, NumberStyles.Float, CultureInfo.InvariantCulture, out var value1);
            var parseVal2Suc = double.TryParse(val2, NumberStyles.Float, CultureInfo.InvariantCulture, out var value2);

            if (!parseVal1Suc || !parseVal2Suc)
                return Messages.InvalidNumberMessage;

            return operation switch
            {
                "Plus" => Calculator.Plus(value1, value2).ToString(CultureInfo.InvariantCulture),
                "Minus" => Calculator.Minus(value1, value2).ToString(CultureInfo.InvariantCulture),
                "Divide" when value2 == 0 => Messages.DivisionByZeroMessage,
                "Divide" => Calculator.Divide(value1, value2).ToString(CultureInfo.InvariantCulture),
                "Multiply" => Calculator.Multiply(value1, value2).ToString(CultureInfo.InvariantCulture),
                _ => Messages.InvalidOperationMessage
            };
        }
    }
}
