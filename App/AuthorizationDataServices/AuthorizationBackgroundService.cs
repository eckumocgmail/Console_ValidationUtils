 
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
 
using System.Timers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Console_Encoder;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Поток управления жизненым циклом обьектов сеанса.
/// Для принятия решения выполняет процедуру проверки подлинности
/// в удостоверяющих центрах.
/// </summary>
public class AuthorizationBackgroundService : IHostedService, IDisposable
{
 
   
    public static void ConfigureAuthorizationBackground(HostBuilderContext context, IServiceCollection services)
    {
        Console.WriteLine("ConfigureAuthorizationBackground(...)");
        services.AddSingleton(typeof(Service), sp =>
        {
            
            var service = context.Configuration.GetSection("Service").Get<Service>();
            if (service != null)
                throw new ArgumentNullException("Не удалось получить сведения о веб-сервисе");
            return service;
        });

        services.AddSingleton<APIUsers, AuthorizationUsers>();
        services.AddSingleton<APIWebServices, AuthorizationServices>();
        services.AddSingleton<EmailService>( );

        services.AddHostedService<AuthorizationBackgroundService>();
        services.AddSingleton(typeof(Service),sp=> context.Configuration.GetSection("InetCertificate").Get<Service>());
        services.AddSingleton(typeof(AuthorizationOptions), (p) => {
            var section = context.Configuration.GetSection(nameof(AuthorizationOptions));
            
            var result = section.Get<AuthorizationOptions>();
            if (result == null)
            {
                throw new Exception("Дополните конфигурацию приложения appsettings.json\n" + new AuthorizationOptions().ToJsonOnScreen());
            }
            return result;
        });
    }

    private System.Timers.Timer _timer;
    private readonly ILogger<AuthorizationBackgroundService> _logger;
    private readonly Service _certificate;
    private readonly AuthorizationOptions _options;
    private readonly APIUsers _users;
    private readonly APIWebServices _services;


    /// <param name="options"> Параметры </param>
    /// <param name="users"> Коллекция авторизованных пользователей </param>
    /// <param name="services"> Коллекция авторизованных служб </param>
    public AuthorizationBackgroundService(
            ILogger<AuthorizationBackgroundService> logger,
            AuthorizationOptions options,
            Service certificate,
            APIUsers users,
            APIWebServices services)
    {
        _logger = logger;
        _certificate = certificate;
        _options = options;
        _users = users;
        _services = services; 
        
    }

    private IEnumerable<byte> GeneratePublicKey(string secret)
    {
        var key = new List<byte>();
        var bitles = new Console_Encoder.BitConverter();
        IEnumerable<bool> code = new TextEncoder().Encode(secret);
        List<bool> buffer = new List<bool>();
        foreach(bool next in code)
        {
            buffer.Add(next);
            if (buffer.Count() == 8)
            {
                key.Add(bitles.ToByte(buffer.ToArray()));
                buffer.Clear();
            }
        }
        key.Add(bitles.ToByte(buffer.ToArray()));
        return key;
    }


    // автивация службы контроля активности
    public async Task StartAsync(CancellationToken stoppingToken)
    {


        _logger.LogInformation("StartAsync()");
        await Task.Run(() => {

            _logger.LogInformation("Таймер запущен");
            _timer = new System.Timers.Timer(_options.CheckTimeout);
            _timer.Elapsed += (Object source, ElapsedEventArgs e) => {
               
                _logger.LogInformation($" Новости: ");
                _logger.LogInformation($" в настоящий момент { _users.GetAll().Count} пользователей; ");
                _logger.LogInformation($" в настоящий момент { _services.GetAll().Count} сервисов; ");

                _users.DoCheck();
                _services.DoCheck();
            };
            _timer.AutoReset = true;
            _timer.Enabled = true;
        });
    }
    

    

    // автивация службы контроля активности
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Stop();
        await Task.CompletedTask;
    }
    public void Dispose() => _timer?.Dispose();
}


       