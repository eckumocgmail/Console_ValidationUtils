using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static System.Console;
using static System.Threading.Thread;
using static System.Threading.Tasks.Task;

/// <summary>
/// IConfigurationBuilder-добавляет источники данных
/// </summary>
public class Factory : LoggerFactory
{
    public static ILogger<T> GetLogger<T>(string v="")
    {
        return Factory.Create().CreateLogger<T>();
    }

}


namespace Microsoft.Extensions.Logging { }
/// <summary>
/// global::LoggerFactory создауёт объекты ILogger
/// </summary>
public class LoggerFactory
{

    private static global::LoggerFactory Instance = null;
    public static global::LoggerFactory Create(Action<ILoggingBuilder> configure) => Create();
    public ILogger<Source> CreateLogger<Source>() => new ConsoleLogger<Source>();
    public ILogger CreateLogger(string name)=> new ConsoleLogger(name);
    public static global::LoggerFactory Create()
    {
        if (Instance == null)
        {
            var instance = new global::LoggerFactory();
            Sleep(100);
            if (Instance == null)
                Instance = instance;
        }
        return Instance;
    }


}


/// <summary>
/// //////////////////////////////////////////////////////////////////////////////////////
/// </summary>

public interface ILogger<T> : ILogger { }
public interface ILogger
{
    public void Info(params object[] messages);
    public void Error(params object[] messages);
    public void Error(Exception ex, params object[] messages);
    public void LogInformation(params object[] messages);
    public void LogDebug(params object[] messages);
    public void LogError(params object[] messages);
    public void LogError(Exception ex, params object[] messages);
    public void LogError(object[] messages, Exception ex);
    public void LogError(object message, Exception ex);

}

public class ConsoleLogger<T> : ConsoleLogger, ILogger<T>
{
    
    public ConsoleLogger() : base(typeof(T).GetTypeName())
    {
    }
}
public class ConsoleLogger : ILogger
{

    protected string _name;

    public ConsoleLogger(string name)
    {
        this._name = name;
    }

    private string GetName() => _name;


    public void Info(params object[] messages) => LogInformation(messages);
    public void Error(params object[] messages) => LogInformation(messages);
    public void Error(Exception ex, params object[] messages) => LogInformation(messages);
    public void LogError(object[] messages, Exception ex) => LogError(ex, messages);
    public void LogError(object message, Exception ex) => LogError(ex, message);
    public void LogError(params object[] messages)=> LogInformation(messages);
    public void LogError(Exception ex, params object[] messages)
    {
        
        LogInformation(messages);
        LogInformation(ex.Message);
        LogInformation(ex.StackTrace);
    }
    public void LogInformation(params object[] messages)
    {
        Write($"\n[{GetName()}]:");
        foreach (var message in messages)   
            WriteLine(message);
    }

    public void LogDebug(params object[] messages)
    {
        LogInformation(messages);
    }
}