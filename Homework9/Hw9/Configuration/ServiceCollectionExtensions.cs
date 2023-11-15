using Hw9.Services.ExpressionParser;
using Hw9.Services.MathCalculator;

namespace Hw9.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services
            .AddTransient<IMathCalculatorService, MathCalculatorService>();
    }

    public static IServiceCollection AddExpressionParser(this IServiceCollection services) 
    { 
        return services.
            AddTransient<IExpressionParserService, ExpressionParserService>();
    }
}