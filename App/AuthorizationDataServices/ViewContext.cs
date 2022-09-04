using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using NetCoreConstructorAngular;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class UserSessionViewContext: Microsoft.AspNetCore.Mvc.Rendering.ViewContext, IDisposable<string,object>
{


    ConcurrentDictionary<int, object> Data { get; set; }
    /*

    public static AgentsHub Agents { get; set; }

    public FlexContainer Root { get; set; } = new FlexContainer();
    public Router Router { get; set; } = new Router();
    public PageItem State { get; set; }
    public Random Random { get; set; } = new Random();
    public ConcurrentDictionary<string, object> Scope { get; set; } = new ConcurrentDictionary<string, object>();
    
    public ConcurrentQueue<ServerTask> ServerTasks { get; set; } = new ConcurrentQueue<ServerTask>();
    public ConcurrentQueue<ClientTask> ClientTasks { get; set; } = new ConcurrentQueue<ClientTask>();
    public ConcurrentDictionary<string, Func<object, object>> CallbackStack { get; set; } = new ConcurrentDictionary<string, Func<object, object>>();
    public Notifications Notifications { get; set; } = new Notifications();
    public string ConnectionId { get; internal set; }

    public ViewContext()
    {
        this[Notifications.GetContentPath()] = Notifications;
        this.Root.Append(new Button());

    }

    /// <summary>
    /// Вычисление уникального ключа в данном контексте
    /// </summary>
    /// <returns> уникальный ключ </returns>
    private string GenerateKey()
    {
        string key = null;
        do
        {
            key = RandomString();
        } while (this.CallbackStack.ContainsKey(key));
        return key;
    }


    public void test()
    {

        
    }

    /// <summary>
    /// Получение случайной последовательности символов
    /// </summary>
    /// <returns> последовательность символов </returns>
    private string RandomString()
    {
        Random random = new Random();
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower() +
                        "0123456789";
        return new string(Enumerable.Repeat(chars, 32)
                            .Select(s => s[random.Next(s.Length)]).ToArray());
    }



    /// <summary>
    /// Регистрация асинхронной операции
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    public string PushCallback(Func<object,object> handle)
    {
        lock (CallbackStack)
        {
            string key = GenerateKey();
            CallbackStack[key] = handle;
            return key;
        }        
    }


    /// <summary>
    /// Получение операции для выполнения
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Func<object, object> TakeCallback(string key)
    {
        if (CallbackStack.ContainsKey(key) == false)
        {
            throw new Exception("Ключ "+key+" не зарегистрирован");
        }
        else
        {
            Func<object, object> res;
            CallbackStack.TryRemove(key, out res);
            return res;
        }
    }




    public List<ServerTask> DequequeServerTasks()
    {
        lock (ServerTasks)
        {
            ServerTask[] messages = ServerTasks.ToArray();
            ServerTasks.Clear();
            return new List<ServerTask>(messages);
        }

    }


    public void AddNotificationMessage(NotificationMessage message)
    {
        
        Notifications.Add(message);        
        
        Notifications.Changed = true;
    }

   
    

    public void Bootstrap( )
    {

    }*/
}