using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Расширения для динамической компиляции
/// </summary>
public static class ObjectCompileExpExtensions
{


    public static object InvokeAny(this IEnumerable<object> items, string act, params object[] arr)
    {
        foreach (var context in items)
        {
            try
            {
                return context.GetType().GetMethods().FirstOrDefault(m => m.Name == act).Invoke(context, arr);
            }
            catch (Exception ex)
            {
                ex.ToString().WriteToConsole();
                continue;
            }
        }
        throw new Exception("Ни один из объектов не смог выполнить процедуру: "+act+ " "+arr.ToJsonOnScreen());
    }

    public static object Invoke(this object context, string act, params object[] arr)
    {
        return context.GetType().GetMethods().FirstOrDefault(m => m.Name == act).Invoke(context, arr);
    }

    public static object Invoke(this object context, string act, IDictionary<string,object> arguments)
    {
        return context.GetType().GetMethods().FirstOrDefault(m => m.Name == act).Invoke(context, arguments.Values.ToArray());
    }

    /// <summary>
    /// Компиляция выражения
    /// </summary>
    public static object Compile(this object context, string expression)
    {
        try
        {
            var res = Expression.Compile(expression, context);
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Не удалось выполнить символьное выражение: {expression} \n {ex.ToString()}");
        }
    }
    /// <summary>
    /// Компиляция выражения
    /// </summary>
    public static string Interpolate(this object context, string expression)
    {
        try
        {
            var res = Expression.Interpolate(expression, context);
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Не удалось выполнить символьное выражение: {expression} \n {ex.ToString()}");
        }
    }


}