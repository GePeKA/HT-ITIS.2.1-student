using Hw10.DbModels;
using Hw10.Services;
using Hw10.Services.CachedCalculator;
using Hw10.Services.ExpressionParser;
using Hw10.Services.ExpressionCalculator;
using Hw10.Services.MathCalculator;

namespace Hw10.Configuration;

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
                s.GetRequiredService<ApplicationContext>(), 
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