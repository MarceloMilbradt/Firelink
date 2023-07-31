using Newtonsoft.Json;

namespace Firelink.App.Shared.Devices;

public class DeviceDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public bool Online { get; set; }
    public bool Power { get; set; }

    public string ProductName { get; set; }
}