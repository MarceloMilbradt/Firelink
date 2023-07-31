using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuyaConnector.Data;

public static class LedCommands
{
    public const string Scene = "scene_data";
    public const string Color = "colour_data";
    public const string Mode = "work_mode";
    public const string Power = "switch_led";
}
public static class LedWorkModes
{
    public const string Color = "colour";
    public const string Scene = "scene";
}

public enum WorkMode
{
    Color,
    Scene
}