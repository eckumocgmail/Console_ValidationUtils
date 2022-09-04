using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

internal class HttpSessionModule
{
    internal static void Configure(WebHostBuilderContext ctx, IServiceCollection services)
    {
        services.AddScoped<CookieManager>();
        services.AddScoped<RequestTransient>();
    }
    internal static void ConfigureBackground(IConfiguration configuration, IServiceCollection services)
    {
        services.AddSingleton<SessionOptions>();
        services.AddSingleton<SessionManager>();        
        services.AddHostedService<SessionBackgroundService>();
    }
}