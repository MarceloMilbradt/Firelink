using System.ComponentModel.DataAnnotations;

namespace Firelink.Presentation.Validation;

public class UrlRequiredIfNotDefaultAttribute : ValidationAttribute
{
    private readonly string _idFieldName;
    private readonly string _defaultIdValue;

    public UrlRequiredIfNotDefaultAttribute(string idFieldName, string defaultIdValue = "Default")
    {
        _idFieldName = idFieldName;
        _defaultIdValue = defaultIdValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Get the ID property from the validation context
        var idProperty = validationContext.ObjectType.GetProperty(_idFieldName);

        if (idProperty == null)
        {
            return new ValidationResult($"Unknown property: {_idFieldName}");
        }

        var idValue = idProperty.GetValue(validationContext.ObjectInstance)?.ToString();

        // Check if ID is different from the default value
        if (idValue != _defaultIdValue)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult("The Url field is required");
            }
        }

        return ValidationResult.Success;
    }
}
