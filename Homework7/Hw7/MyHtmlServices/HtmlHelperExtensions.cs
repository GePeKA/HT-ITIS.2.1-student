using Hw7.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var builder = new HtmlContentBuilder();

        var modelType = helper.ViewData.ModelExplorer.ModelType;
        var model = helper.ViewData.ModelExplorer.Model;

        foreach (var property in modelType.GetProperties())
        {
            builder.AppendLine(EditProperty(property, model));
        }

        return builder;
    }

    private static IHtmlContent EditProperty(PropertyInfo property, object? model)
    {
        var builder = new HtmlContentBuilder().AppendHtmlLine("<div>");

        builder.AppendLine(CreateLabel(property))
            .AppendLine(AddInput(property, model))
            .AppendLine(AddValidationMessage(property, model)); 

        return builder.AppendHtmlLine("</div> <br>");
    }

    private static IHtmlContent CreateLabel(PropertyInfo property)
    {
        string labelText;

        if (property.GetCustomAttribute<DisplayAttribute>() != null)
            labelText = property.GetCustomAttribute<DisplayAttribute>()!.Name!;
        else
            labelText = property.Name.SplitByCamelCase();

        return new HtmlContentBuilder().AppendHtmlLine($"<label for=\"{property!.Name}\">{labelText}</label>: ");
    }

    private static IHtmlContent AddInput(PropertyInfo property, object? model)
    {
        var builder = new HtmlContentBuilder();
        var propertyType = property.PropertyType;
        var value = model == null ? "" : property.GetValue(model)?.ToString();

        if (propertyType == typeof(int?))
            builder.AppendHtmlLine(CreateInputField("number", property.Name, value));
        else if (propertyType == typeof(string))
            builder.AppendHtmlLine(CreateInputField("text", property.Name, value));
        else if (propertyType.IsEnum)
            builder.AppendLine(CreateSelectForEnum(property, model));

        return builder;
    }

    private static string CreateInputField(string inputType, string propertyName, string value)
    {
        return $"<input type=\"{inputType}\" id=\"{propertyName}\" name=\"{propertyName}\" value=\"{value}\">";
    }

    private static IHtmlContent CreateSelectForEnum(PropertyInfo property, object? model)
    {
        var builder = new HtmlContentBuilder();
        var enumType = property.PropertyType;

        var selected = model != null ? property.GetValue(model) : null;

        builder.AppendHtmlLine($"<select name=\"{property.Name}\" id=\"{property.Name}\">");

        foreach(var field in Enum.GetValues(enumType))
        {
            var sel = "";
            if (selected != null && selected.ToString() == field.ToString())
                sel = "selected";

            builder.AppendHtmlLine($"<option value=\"{field}\" {sel}>{field}</option>");
        }

        return builder.AppendHtmlLine("</select>");
    }

    private static IHtmlContent AddValidationMessage(PropertyInfo property, object? model)
    {
        var builder = new HtmlContentBuilder();

        builder.AppendHtmlLine("<span class=\"text-danger\">");
        var validationAttributes = property.GetCustomAttributes<ValidationAttribute>(true);

        foreach (var attribute in validationAttributes)
        {
            if (model!= null && !attribute.IsValid(property.GetValue(model)))
            {
                builder.AppendHtml(attribute!.ErrorMessage);
                break;
            }
        }

        return builder.AppendHtmlLine("</span>");
    }

    [ExcludeFromCodeCoverage]
    private static string SplitByCamelCase(this string str)
    {
        return System.Text.RegularExpressions.Regex.Replace(str,
            "([A-Z])", " $1"
            ).Trim();
    }
} 