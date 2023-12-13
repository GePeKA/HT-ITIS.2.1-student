using System.Diagnostics.CodeAnalysis;
using Hw13.Calculator.Configuration;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddExpressionParser()
    .AddExpressionCalculator()
    .AddMathCalculator()
    .AddCachedMathCalculator();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculator}/{action=Index}/{id?}");
app.Run();

namespace Hw13.Calculator
{
    [ExcludeFromCodeCoverage]
    public partial class Program { }
}