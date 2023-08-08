using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace App.Extensions;

[ExcludeFromCodeCoverage]
public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilderInitial<T, TProperty> ruleBuilder)
    {
        DefaultValidatorOptions.Configurable(ruleBuilder).CascadeMode = CascadeMode.Stop;
            
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");
    }
    
    public static IRuleBuilderOptions<T, string> FileExists<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must<T, string>((Func<T, string, bool>) ((x, val) => File.Exists(val)))
            .WithMessage("File {PropertyName} does not exist.");
    }
    
    public static IRuleBuilderOptions<T, string> DirectoryExists<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must<T, string>((Func<T, string, bool>) ((x, val) => Directory.Exists(val)))
            .WithMessage("Directory {PropertyName} does not exist.");
    }

    public static IRuleBuilderOptions<T, TProperty> In<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, params TProperty[] values)
    {
        if (values?.Any() != true)
        {
            throw new ArgumentException("At least one value is expected", nameof(values));
        }
        
        return ruleBuilder
            .Must(values.Contains)
            .WithMessage($"{{PropertyName}} must be one of these values '{string.Join(",", values)}'.");
    }
}