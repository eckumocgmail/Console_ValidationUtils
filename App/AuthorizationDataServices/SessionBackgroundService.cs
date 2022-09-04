using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

 

 

public class SessionBackgroundService : BackgroundService, IDisposable
{
    private readonly SessionManager _application;
    private readonly AuthorizationOptions _options;
    private static readonly IEventsPool _events = new UserEventsPool();

    public SessionBackgroundService( SessionManager application, AuthorizationOptions options )
    {
        this._application = application;
        this._options = options;
        
    }


    /// <summary>
    /// Выполнение фоновой задачи до тех пор пока не получен запрос на прерывание
    /// </summary>
    /// <param name="stoppingToken"> токен управления потоком </param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {             
            _events.Recieve(_options.CheckTimeout);
            await this.DoCheck();
        }
    }


    /// <summary>
    /// Выполнение проверки активности обьектов 
    /// </summary>        
    private async Task DoCheck()
    {
        if(_options.LogginAuth)
        {
            Writing.ToConsole("Check point");
        }            
        _application.DoCheck(_options.SessionTimeout);
        await Task.CompletedTask;
    }



    /// <summary>
    /// Уничтожение сервиса
    /// </summary>
    public override void Dispose()
    {           
        base.Dispose();
    }
         
}


