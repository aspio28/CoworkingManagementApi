using FluentValidation;

namespace CoworkingManagement.Application.Common.Extensions;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string?> NotContainHtml<T>(this IRuleBuilder<T, string?> ruleBuilder, string propertyName)
    {
        return ruleBuilder
            .Must(text =>
            {
                if (string.IsNullOrEmpty(text)) return true;

                return !text.Contains('<') && !text.Contains('>') && !text.Contains('/');
            })
            .WithMessage($"{propertyName} contains invalid characters (HTML tags are not allowed).");
    }
}