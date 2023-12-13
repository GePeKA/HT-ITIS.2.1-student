using Hw13.Calculator.Services;
using Hw13.Calculator.Services.CachedCalculator;
using Hw13.Calculator.Services.ExpressionParser;
using Hw13.Calculator.Services.ExpressionCalculator;
using Hw13.Calculator.Services.MathCalculator;
using Microsoft.Extensions.Caching.Memory;

namespace Hw13.Calculator.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services.AddTransient<MathCalculatorService>();
    }
    
    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        return services.AddScoped<IMathCalculatorService>(s =>
            new MathCachedCalculatorService(
                s.GetRequiredService<IMemoryCache>(), 
                s.GetRequiredService<MathCalculatorService>()));
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