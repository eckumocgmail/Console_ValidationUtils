using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// расширения для ввода данных
/// </summary>
public static class ObjectInputExtensions
{
    public static IConfiguration RunConfigurationBilder(this object target)
    {
        return ConsoleProgram.Run(target);
    }


    public static IConfiguration ToConfiguration(this object target)
    {
        target.Info($"ToConfiguration()");
        
        string filepath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), target.GetFileName()).ToString();
        target.Info(filepath);

        string json = target.ToJson();
        target.Info(json);

        var builder = new ConfigurationBuilder();        
        target.ToJsonOnScreen().WriteToFile( filepath );
        builder.AddJsonFile(target.GetFileName());
        return builder.Build();
    }
     

    

    /// <summary>
    /// Получение имён свойств полежащих вводу
    /// </summary>
    public static string[] GetUserInputPropertyNames(this Type type)
    {
        var navs = Utils.GetRefTypPropertiesNames(type);        
        return ReflectionService
              .GetPropertyNames(type).
               Where(n => 
                   Utils.IsInput(type, n) &&
                   Utils.IsVisible(type, n) &&
                   navs.Contains(n) == false && 
                   navs.Contains(n+"ID") == false).ToArray();
    }

    /// <summary>
    /// Проверка является ли свойство коллекцией
    /// </summary>
    public static bool IsCollectionType(this Type type)
    {
        return Typing.IsCollectionType(type);
    }

    /// <summary>
    /// Получение аттрибутов
    /// </summary> 
    public static Dictionary<string, string> GetAttrs( this object p )
    {
        if(p is Type)return Utils.ForType((Type)p);
        return Utils.ForType(p.GetType());
    }

    /// <summary>
    /// Получение аттрибутов
    /// </summary> 
    public static string GetAttribute(this object p, string nameoftype)
    {
         
        var attrs = p.GetAttrs();
        if (nameoftype.EndsWith("Attribute"))
        {
            if (attrs.ContainsKey(nameoftype))
            {
                return attrs[nameoftype];
            }
            else if (attrs.ContainsKey(nameoftype.Replace("Attribute", "")))
            {
                return attrs[nameoftype.Replace("Attribute", "")];
            }
            else
            {
                return null;
            }
        }
        else
        {
            if (attrs.ContainsKey(nameoftype))
            {
                return attrs[nameoftype];
            }
            else if (attrs.ContainsKey(nameoftype+"Attribute"))
            {
                return attrs[nameoftype + "Attribute"];
            }
            else
            {
                return null;
            }
        }
        
    }


    /// <summary>
    /// Получение имён свойств
    /// </summary> 
    public static string[] GetOwnPropertyNames(this Type type)
    {
        return (from p in new List<PropertyInfo>((type).GetProperties()) where p.DeclaringType == type select p.Name).ToArray();
    }

    /// <summary>
    /// Получение имён методов
    /// </summary> 
    public static string[] GetOwnMethodNames(this Type type)
    {
        return (from p in new List<MethodInfo>((type).GetMethods()) where p.DeclaringType == type select p.Name).ToArray();
    }

    /// <summary>
    /// Возвращает значение свойства
    /// </summary> 
    public static object GetProperty(this object target, string property )
    {
        var prop = target.GetType().GetProperties().FirstOrDefault(n => n.Name == property);
        var result = prop == null? null: prop.GetValue(target);
        return result;
    }

    /// <summary>
    /// Возвращает значение свойства
    /// </summary> 
    public static void SetProperty(this object target, string property, object value)
    {
        target.GetType().GetProperty(property).SetValue(target,value);
    }

    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static string ToJsnop(this object target)
    {
        return Formating.ToJson(target);
    }


    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static Dictionary<string, object> ToDictionary(this object target)
    {
        var result = new Dictionary<string, object>();
        target.GetOwnPropertyNames().ToList().ForEach(name => { 
            result[name]=target.GetProperty(name);  
        });
        return result;
    }


    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static Dictionary<string, string> ToActionLinks(this object target, string pattern)
    {
        var result = new Dictionary<string, string>();
        target.GetOwnPropertyNames().ToList().ForEach(name => {
            result[name] = Expression.Interpolate(pattern, new { 
                action = name                
            });
        });
        return result;
    }

    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static string ToXML(this object target)
    {
        return Formating.ToXML(target);
    }
    
    /// <summary>
    /// Преобразование к формату JSON
    /// </summary>
    public static string ToJsonOnScreen(this object target)
    {
        
        return JsonConvert.SerializeObject(target,new JsonSerializerSettings() { Formatting=Formatting.Indented});
    }
   
}