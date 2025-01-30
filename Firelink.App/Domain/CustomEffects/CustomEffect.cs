using MemoryPack;

namespace Firelink.Domain.CustomEffects;

[MemoryPackable]
public partial class CustomEffect
{
    public string? ItemId { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }
    public int PresetId { get; set; }
    public int EffectId { get; set; }
    public int PaletteId { get; set; }
    public ConfigurationType EffectType { get; set; }
}

