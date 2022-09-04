using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Console_Validation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ReCaptcha;

/// <summary>
/// Предварительно выполняется тестироание:
/// 
/// </summary>
public class ValidationProgram : TestingUnit
{
    private static void ConfigureHost(IWebHostBuilder webBuilder)
    {
        webBuilder.UseStartup<ValidationProgram>();
        webBuilder.ConfigureServices((ctx, services) => {
            HttpSessionModule.Configure(ctx, services);
            ReCaptchaModule.Configure(ctx.Configuration, services);
            AuthorizationDataModel.Configure(ctx.Configuration, services);
        });
        
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();
        services.AddControllersWithViews();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddSession();
        services.AddRazorPages();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseAction(LogHttpRequest);
        app.UseDeveloperExceptionPage();
        app.UseHttpsRedirection();
        app.UseSession();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
            endpoints.MapFallbackToPage("/Http404");
        });
        app.UseStaticFiles(WwwRootConfiguration);
    }

    private static ILogger<ValidationProgram> Logger { get; } = Factory.GetLogger<ValidationProgram>();
    private static IConfiguration Configuration { get; set; }
    private static StaticFileOptions WwwRootConfiguration { get; set; } 
    private static string Dir { get; set; }


    private static IHostBuilder HostBuilder = CreateHostBuilder();


    public static void Main(string[] args)
    {
        Logger.LogInformation("Main()");
        
        try
        {
            IHost host = HostBuilder.Build();
            host.Run();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.GetType().Name + "Ошибка при запуске: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
            foreach (string word in ex.Message.SplitWords().Where(word => Assembly.GetExecutingAssembly().GetTypes().Select(t => t.GetTypeName()).Contains(word)))
            {
                Logger.LogError(word);
            }
        }
    }


    /// <param name="configuration"></param>
    public ValidationProgram(IConfiguration configuration)
    {
        
        Configuration = configuration;
        Dir = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot");
        if (System.IO.Directory.Exists(Dir) == false)        
            System.IO.Directory.CreateDirectory(Dir);        
        
        WwwRootConfiguration = new StaticFileOptions() { 
            FileProvider = new PhysicalFileProvider(Dir),
            RequestPath = "",            
        };

        DoTest().ToDocument().WriteToConsole();
    }

    public override TestingReport DoTest()
    {
        Push(new AttributeControlsTests());
        Push(new AttributeInputTests());
        Push(new AttributeValidationTests());
        Push(new DataModuleTest());
        Push(new ExtensionUtilsTests());
        Push(new CommonTextUtilsTests());         
        Push(new CoreContextTests());
        Push(new ConsoleProgramTest());
        //Push(new AuthModuleTests());

        return base.DoTest();
    }

    public static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(ConfigureHost)
            .ConfigureServices((ctx,services) => {

                Logger.LogDebug("Регистрация фоновой службы HttpSessionModule");

                HttpSessionModule.ConfigureBackground(ctx.Configuration,services);
            })
            .ConfigureServices((ctx, services) => {

                Logger.LogDebug("Регистрация фоновой службы AuthorizationBackgroundService");
                AuthorizationBackgroundService.ConfigureAuthorizationBackground(ctx, services);
            });

    private void LogHttpRequest(HttpContext http)
    {
        Factory.GetLogger<ValidationProgram>().LogInformation(http.Connection.LocalIpAddress.ToString());
    }
}



public static class IApplicationBuilderExtensions
{
    
    public static IHostBuilder ConfigureBackgroundServices(this IHostBuilder builder)
    {
        
        builder.ConfigureBackgroundServices();
        return builder;
    }
    public static void UseAction(this IApplicationBuilder app, Action<HttpContext> action)
    {
        app.Use((http, next) => {
            action(http);
            return next.Invoke();
        });
    }

  
}
