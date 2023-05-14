using Firelink.App.Server;
using Firelink.App.Server.Features.Devices;
using Firelink.App.Server.Features.Spotify.Player;
using Firelink.App.Server.Features.Spotify.Track;
using Firelink.Application.Common.Interfaces;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddEndpoints(typeof(IEndpointDefinition));
        services.AddSignalR();
        services.AddResponseCompression(options =>
            options.MimeTypes = ResponseCompressionDefaults
                .MimeTypes
                .Concat(new[] { "application/octect-stream" })
        );
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        
        services.AddLogging(loggingBuilder =>loggingBuilder.AddSerilog(Log.Logger, dispose: true));
        services.AddTransient<ITrackHubContext, TrackHubContext>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Firelink API",
                Version = "v1"
            });
        });

        services.AddHostedService<PlayerListener>();
        services.AddHostedService<TurnOnDeviceOnStartup>();
        return services;
    }
}