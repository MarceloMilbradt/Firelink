using Firelink.Domain.CustomEffects;
using System.ComponentModel.DataAnnotations;
using Firelink.Presentation.Components.Pages.CustomEffects;

namespace Firelink.Presentation.Validation;

public class RequiredIfType : ValidationAttribute
{

    private readonly ConfigurationType _type;

    public RequiredIfType(ConfigurationType type)
    {
        _type = type;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var model = (CustomEffectModel)validationContext.ObjectInstance;
        if (model.Type == _type &&  (value == null || (int)value < 0) )
        {
            return new ValidationResult(ErrorMessage ?? $"This Field is required when the type is {_type}.");
        }

        return ValidationResult.Success;
    }
}
