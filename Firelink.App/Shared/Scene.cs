using Newtonsoft.Json;

namespace Firelink.App.Shared;

public record Scene
{
    [JsonProperty("scene_num")]
    public int SceneNum { get; set; }
    [JsonProperty("scene_units")]
    public IEnumerable<SceneUnit> SceneUnits { get; set; }
}
