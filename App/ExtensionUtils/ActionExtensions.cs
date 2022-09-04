﻿using System;
using System.Timers;


/// <summary>
/// Расширения методов выполнения делегатов
/// </summary>
public static class ActionExtensions
{

    /// <summary>
    /// Выполнение делегата по истечению заданного в милисекундах промежутка времени
    /// </summary>
    /// <param name="action"> делегат </param>
    /// <param name="ms"> кол-во миллисекунд </param>
    public static void SetTimeout( this Object context,  Action action, long ms)
    {
        System.Timers.Timer aTimer = new System.Timers.Timer(ms);
        aTimer.Elapsed += (Object source, ElapsedEventArgs e) => {
            action();
            aTimer.Enabled = false;
        };
        aTimer.AutoReset = false;
        aTimer.Enabled = true;
    }



    /// <summary>
    /// Выполнение с заданным интервалом определённое кол-во раз
    /// </summary>
    /// <param name="actionsCount"></param>
    /// <param name="actionTimeout"></param>
    /// <param name="p"></param>
    public static void Simulate(this Object context, Action todo, int actionTimeout, int actionsCount)
    {
        for(int i=1; i<=actionsCount; i++)
        {
            context.SetTimeout(todo, actionTimeout*i);
        }
    }


    /// <summary>
    /// Перехват выполнения исключений
    /// </summary> 
    public static Action Catch(this Object context, Action todo, Action<Exception> catcher)
    {
        return () =>
        {
            try
            {                
                todo();
            }
            catch(Exception ex)
            {
                catcher(ex);
            }
        };
    }


    /// <summary>
    /// Выполнение делегата с заданным интервалом в милисекундах
    /// </summary>
    /// <param name="action"> делегат </param>
    /// <param name="ms"> кол-во миллисекунд </param>
    public static void SetInterval(this Object context, Action action, long ms)
    {
        System.Timers.Timer aTimer = new System.Timers.Timer(ms);
        aTimer.Elapsed += (Object source, ElapsedEventArgs e) => {
            action();
              
        };
        aTimer.AutoReset = true;
        aTimer.Enabled = true;            
    }
}
