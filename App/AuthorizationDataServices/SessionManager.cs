using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Управление сеансом
/// </summary>
public class SessionManager
{
    private ConcurrentDictionary<string, SessionContext> connections = 
        new ConcurrentDictionary<string, SessionContext>();


    /// <summary>
    /// Получение контекста сеанса
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SessionContext GetById( string id )
    {
        if(connections.ContainsKey(id))
        {
            connections[id].Timestamp = GetTimestamp();
            return connections[id];
        }
        else
        {
            return connections[id] = new SessionContext(GetTimestamp());
        }
    }


    /// <summary>
    /// Проверка наличия
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    internal bool ContainsKey(string id)
    {
        return connections.ContainsKey(id);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeout"></param>
    internal IDictionary<string, object[]> DoCheck(long timeout)
    {
        IDictionary<string, object[]> results = new Dictionary<string, object[]>();
        this.connections.Keys.ToList().ForEach(System.Console.WriteLine);
        lock (connections)
        {
            

            List<string> removeList = new List<string>();
            foreach (var pair in connections)
            {
                object[] locals = null;
                lock (pair.Value.Events)
                {

                    locals = pair.Value.Events.ToArray();
                }
                results[pair.Key] = locals;
                pair.Value.Events.Clear();
                SessionContext session = pair.Value;
                if ((GetTimestamp() - session.Timestamp) > timeout)
                {
                    removeList.Add(pair.Key);
                }
                else
                {
                    //Writing.ToConsole($"Обновление сеанса: {session.GetHashCode()}"); 
                   
                }
            }
            foreach (string key in removeList)
            {
                Invalidate(key);
            }
        }
        return results;
    }


    /// <summary>
    /// Уничтожение сессии
    /// </summary>
    /// <param name="key"></param>
    public void Invalidate(string key)
    {
        
        if (connections.ContainsKey(key))
        {
            SessionContext session = null;
            connections.Remove(key, out session);
            if (session != null)
                session.Dispose();
        }     
        
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal long GetTimestamp()
    {
        return (long)(((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0))).TotalMilliseconds);
    }
}
