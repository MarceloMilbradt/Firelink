namespace Firelink.App.Shared;

public static class ScenePresets
{
    public static Scene Rainbow = new()
    {
        SceneNum = 102,
        SceneUnits =
       [
            new SceneUnit(0, 1000, 1000, "gradient",  50, 50),
                new SceneUnit(60, 1000, 1000, "gradient", 50, 50),
                new SceneUnit(120, 1000, 1000, "gradient", 50, 50),
                new SceneUnit(180, 1000, 1000, "gradient", 50, 50),
                new SceneUnit(240, 1000, 1000, "gradient", 50, 50),
                new SceneUnit(300, 1000, 1000, "gradient", 50, 50),
            ]
    };
}