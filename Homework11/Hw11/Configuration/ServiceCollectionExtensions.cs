using Hw11.Services.ExpressionCalculator;
using Hw11.Services.ExpressionParser;
using Hw11.Services.MathCalculator;

namespace Hw11.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services.AddTransient<IMathCalculatorService, MathCalculatorService>();
    }

    public static IServiceCollection AddExpressionParser(this IServiceCollection services)
    {
        return services
            .AddTransient<IExpressionParserService, ExpressionParserService>();
    }

    public static IServiceCollection AddExpressionCalculator(this IServiceCollection services)
    {
        return services
            .AddTransient<IExpressionCalculatorService, ExpressionCalculatorService>();
    }
}