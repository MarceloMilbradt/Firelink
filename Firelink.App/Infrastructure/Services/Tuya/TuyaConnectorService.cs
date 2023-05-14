﻿using Firelink.Application.Common.Interfaces;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TuyaConnector;
using TuyaConnector.Data;
using TuyaConnector.Data.Settings;

namespace Firelink.Infrastructure.Services.Tuya;

public class TuyaConnectorService : ITuyaConnector
{
    private readonly ITuyaClient _tuyaClient;
    private readonly string _userId;
    private readonly IAppCache _cache;

    public TuyaConnectorService(ILogger<ITuyaClient> logger, IConfiguration configuration, IAppCache cache)
    {
        _cache = cache;
        var tuyaConfig = configuration.GetSection("Tuya");
        var clientId = tuyaConfig["ClientId"]!;
        var clientSecret = tuyaConfig["Secret"]!;
        _userId = tuyaConfig["UserId"]!;
        _tuyaClient = TuyaClient.GetBuilder()
            .UsingDataCenter(DataCenter.WestUs)
            .UsingClientId(clientId)
            .UsingSecret(clientSecret)
            .UsingLogger(logger)
            .Build();
    }

    public async Task<IEnumerable<Device>> GetUserDevices(CancellationToken cancellationToken)
    {
        return await _cache.GetOrAddAsync($"userId-{_userId}",
            async () => await _tuyaClient.DeviceManager.GetDevicesByUserAsync(_userId, cancellationToken)!,
            new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddHours(24) })!;
    }

    public async Task<Device> GetDevice(string deviceId, CancellationToken cancellationToken)
    {
        return await _cache.GetOrAddAsync($"deviceId-{deviceId}",
            async () => await _tuyaClient.DeviceManager.GetDeviceAsync(deviceId, cancellationToken)!,
            new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddHours(12) })!;
    }

    public async Task SendCommandToDevice(string deviceId, Command command, CancellationToken cancellationToken)
    {
        await _tuyaClient.DeviceManager.SendCommandAsync(deviceId, command, cancellationToken);
    }
}