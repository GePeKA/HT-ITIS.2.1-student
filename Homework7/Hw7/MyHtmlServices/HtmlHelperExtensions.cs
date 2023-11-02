using Hw7.Models;
using Hw7.Models.ForTests;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper, BaseModel model)
    {
        var builder = new HtmlContentBuilder();

        AddTextBoxForProperty(helper, model, builder, "FirstName");
        AddTextBoxForProperty(helper, model, builder, "LastName");
        AddTextBoxForProperty(helper, model, builder, "MiddleName");
        AddTextBoxForProperty(helper,  model, builder, "Age", "number");

        builder.AppendHtml(helper.Label("Sex",
            model.GetType().GetProperty("Sex")?.GetCustomAttribute<DisplayAttribute>()?.Name));

        builder.AppendLine(helper.DropDownList("Sex", helper.GetEnumSelectList<Sex>()));

        return builder;
    }

    [ExcludeFromCodeCoverage]
    private static HtmlContentBuilder AddTextBoxForProperty(IHtmlHelper helper, BaseModel model,
        HtmlContentBuilder builder, string propertyName, string type = "text")
    {
        var labelText = model.GetType().GetProperty(propertyName)?.GetCustomAttribute<DisplayAttribute>()?.Name
            ?? typeof(BaseModel).GetProperty(propertyName)?.Name.SplitByCamelCase();

        builder.AppendHtmlLine($"<label for=\"{propertyName}\">{labelText}");

        builder.AppendLine(helper.TextBox(propertyName, "", new { @type = type }));
        builder.AppendLine(helper.ValidationMessage(propertyName, new { @class = "text-danger" }));

        builder.AppendHtmlLine("</label>");
        builder.AppendHtmlLine("<br> <br>");

        return builder;
    }

    [ExcludeFromCodeCoverage]
    private static string SplitByCamelCase(this string str)
    {
        return System.Text.RegularExpressions.Regex.Replace(str,
            "([A-Z])", " $1"
            ).Trim();
    }
} 