using Firelink.Domain.CustomEffects;
using Firelink.Presentation.Validation;
using System.ComponentModel.DataAnnotations;

namespace Firelink.Presentation.Components.Pages.CustomEffects;

public class CustomEffectModel
{
    [UrlRequiredIfNotDefault(nameof(Id))]
    [Url]
    public string? Url { get; set; }
    public string? Id { get; set; }
    [RequiredIfType(ConfigurationType.Preset, ErrorMessage = "The Id of the Preset is Required")]
    public int? PresetId { get; set; }
    [RequiredIfType(ConfigurationType.Effect, ErrorMessage = "The Id of the Effect is Required")]
    public int? EffectId { get; set; }
    [RequiredIfType(ConfigurationType.Effect, ErrorMessage = "The Id of the Color Palette is Required")]
    public int? PaletteId { get; set; } = 3;
    [Required]
    public ConfigurationType Type { get; set; } = ConfigurationType.Effect;
}
