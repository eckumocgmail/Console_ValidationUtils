using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Reflection;

/// <summary>
/// Расширения для динамической компиляции
/// </summary>
public static class ObjectLogExtensions
{


    public static void Init(this object target, IServiceProvider provider)
    {
        target.InitProperties(provider);
        target.InitFields(provider);
    }
    public static void InitFields(this object target, IServiceProvider provider)
    {
        foreach(FieldInfo field in target.GetType().GetFields())
        {
            
            Dictionary<string, string> attrs = Utils.ForField(target.GetType(), field.Name);
            bool has = Utils.HasAttribute<ServiceAttribute>(attrs);
            if (has)
            {
                try
                {
                    field.SetValue(target, provider.GetService(field.FieldType));
                    target.Info($"{field.Name} успешно инициаллизировано");
                }
                catch(Exception ex)
                {
                    target.Error($"{field.Name} не инициаллизировано");
                    target.Error(ex);
                }
            }
            else
            {
                target.Info($"{field.Name} не требует инициаллизации");
            }
        }
    }
    public static void InitProperties(this object target, IServiceProvider provider)
    {
        
        Typing.ForEachType(target.GetType(), (next) =>
        {
            target.Info(new
            {
                Name = next.GetTypeName().ToJsonOnScreen(),
                Properties = next.GetProperties().Select(p => p.Name).ToJsonOnScreen()
            });
            foreach (PropertyInfo property in next.GetProperties())
            {

                Dictionary<string, string> attrs = Utils.ForProperty(target.GetType(), property.Name);
                bool has = Utils.HasAttribute<ServiceAttribute>(attrs);
                if (has)
                {
                    try
                    {

                        var value = provider.GetService(property.PropertyType);
                        if (value == null)
                        {
                            throw new Exception($"Поставщик {provider.GetTypeName()} не предоставил сервис типа {property.PropertyType.GetTypeName()}");
                        }
                        property.SetValue(target, value);
                        target.Info($"{property.Name} успешно инициаллизировано");
                    }
                    catch (Exception ex)
                    {
                        target.Error($"{property.Name} не инициаллизировано");
                        target.Error(ex);
                    }
                }
                else
                {
                    target.Info($"{property.Name} не требует инициаллизации");
                }
            }
        });
        
    }
    public static void Info(this object target, params object[] messages) => target.LogInformation(messages);
    public static void Error(this object target, params object[] messages) => target.LogInformation(messages);
    public static void Error(this object target, Exception ex, params object[] messages) => target.LogInformation(messages);
    public static void LogError(this object target, object[] messages, Exception ex) => target.LogError(ex, messages);
    public static void LogError(this object target, object message, Exception ex) => target.LogError(ex, message);
    public static void LogError(this object target, params object[] messages) => target.LogInformation(messages);
    public static void LogError(this object target, Exception ex, params object[] messages)
    {

        target.LogInformation(messages);
        target.LogInformation(ex.Message);
        target.LogInformation(ex.StackTrace);
    }
    public static string GetTypeName(this object target)
    {
        return target.GetType().GetTypeName();
    }
    public static string GetFileName(this object target)
    {
        return target.GetTypeName().ToKebabStyle() + ".json";
    }
    public static string GetId(this object target)
    {
        return $"[{target.GetTypeName()}].[{target.GetHashCode()}]";
    }
    public static void LogInformation(this object target, params object[] messages)
    {
        try
        {
            Write($"\n[{target.GetId()}]:\n");
            foreach (var message in messages)
            try
            {
                    if(message.IsPrimitiveType() == false)
                    {
                        Write(message.ToJsonOnScreen());
                    }
                    else
                    {
                        WriteLine(message);
                    }
                    
            }
            catch (Exception ex)
            {
                throw new Exception($"{target.GetId()} не удалось записать: {message} \n {ex.Message} \n {ex.ToString()}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"{target.GetId()} не удалось записать: {messages.ToJson()} \n {ex.Message}\n {ex.ToString()}");
        }
        
            
    }


}