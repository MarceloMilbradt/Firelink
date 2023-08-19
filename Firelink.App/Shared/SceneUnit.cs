using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Firelink.App.Shared;

public record SceneUnit : Hsv
{
    [JsonProperty("bright")]
    public int Bright { get; set; }
    [JsonProperty("temperature")]
    public int Temperature { get; set; }
    [JsonProperty("unit_change_mode")]
    public string UnitChangeMode { get; set; }
    [JsonProperty("unit_gradient_duration")]
    public int UnitGradientDuration { get; set; }
    [JsonProperty("unit_switch_duration")]
    public int UnitSwitchDuration { get; set; }
    public SceneUnit() { }
    public SceneUnit(int h, int s, int v, string unitChangeMode, int unitGradientDuration, int unitSwitchDuration)
    {
        H = h;
        S = s;
        V = v;
        UnitChangeMode = unitChangeMode;
        UnitGradientDuration = unitGradientDuration;
        UnitSwitchDuration = unitSwitchDuration;
    }
    public SceneUnit(string changeMode, int duration, Hsv hsv)
    {
        H = hsv.H;
        S = hsv.S;
        V = hsv.V;
        UnitChangeMode = changeMode;
        UnitGradientDuration = duration;
        UnitSwitchDuration = duration;
    }
    public static SceneUnit FromHsv(Hsv hsv)
    {
        return new SceneUnit
        {
            H = hsv.H,
            S = hsv.S,
            V = hsv.V,
        };
    }
}