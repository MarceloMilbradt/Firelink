using Firelink.Presentation;
using Firelink.Infrastructure;
using Firelink.Application;
using Firelink.Presentation.Components;
using Firelink.Presentation.Endpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

Log.Information("Starting up!");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapAuthEndpoints();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    throw;
}
finally
{
    Log.Information("Closing Application");
    Log.CloseAndFlush();
}