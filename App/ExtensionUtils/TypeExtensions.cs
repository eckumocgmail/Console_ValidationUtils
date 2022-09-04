using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static CustomizationAttributes;

/// <summary>
/// расширения типов
/// </summary>
public static class TypeExtensions
{
    public static bool IsComponent(this Type type ) => type.Name.EndsWith("Component");
    public static bool IsContainer(this Type type ) => type.Name.EndsWith("Container");
    public static Func<object,Func<IDictionary<string,object>,object>> GetAction(this Type type, string name )
    {
        var method = type.GetMethods().Where(m => m.Name == name).First();
        return (target) => {
            var local = target.ToDictionary();
            return (IDictionary<string, object>  scope) =>
            {
                var transclusion = new Dictionary<string, object>();
                foreach (var kv in local)
                    transclusion[kv.Key] = kv.Value;

                foreach (var kv in scope)
                    transclusion[kv.Key] = kv.Value;

                var started = DateTime.Now;
                var pars = new Dictionary<string, object>();
                var args = new List<object>();
                object result = null;
                try
                {
                    foreach (var name in method.GetParameters().Select(p => p.Name))
                    {
                        if (transclusion.ContainsKey(name))
                        {
                            args.Add(transclusion[name]);
                            pars[name] = transclusion[name];
                        }
                        else
                        {
                            throw new ArgumentException(name);
                        }
                    }

                }
                catch (Exception ex)
                {
                    return MethodResult.OnFailed(ex);
                }
                try
                {
                    
                    result = method.Invoke(target, args.ToArray());
                    return MethodResult.OnComplete(result, pars, started);
                }
                catch (Exception ex)
                {
                    target.Error(ex);
                    return MethodResult.OnFailed(ex, started);
                }
                
            };
        };
    }

    public static bool HasAttributeForField<T>( this Type type, string name ) where T: Attribute => Utils.HasAttribute<T>(Utils.ForField(type, name));
    public static bool HasAttributeForProperty<T>( this Type type, string name ) where T: Attribute => Utils.HasAttribute<T>(Utils.ForField(type, name));

    public static string ToDocument( this Type type)
        => new TypeDocumentation(type).ToJsonOnScreen();

    public static Type[] GetParamTypes(this Type type)
    {
        return type.GenericTypeArguments;
    }
    public static bool IsProperty(this Type type, string name)
        => type.GetProperties().Any(p => p.Name == name);
    public static bool IsMethod(this Type type, string name)
        => type.GetMethods().Any(p => p.Name == name);

    public static bool IsExtends(this System.Object type, Type baseType)
    {
        if (type is Type)
            return ((Type)type).IsExtendsFrom(baseType);
        else return type.GetType().IsExtendsFrom(baseType);
    }
    public static bool IsImplements(this System.Object type, Type baseType)
    {
        if (type is Type)
            return ((Type)type).IsImplementsFrom(baseType);
        else return type.GetType().IsImplementsFrom(baseType);
    }
    public static bool IsExtendsFrom(this Type type, Type baseType)
    {
        return Typing.IsExtendedFrom(type, baseType.Name);
    }
    public static bool IsImplementsFrom(this Type type, Type baseType)
    {
        return Typing.IsImplementedFrom(type, baseType.Name);
    }
    public static bool IsExtendsFrom(this Type type, string baseType)
    {
        return Typing.IsExtendedFrom(type, baseType);
    }
    public static IEnumerable<Type> GetExtendingTypes(this Type type)
    {
        var types = new List<Type>();
        Typing.ForEachType(type, (next) =>
        {
            types.Add(next);
        });
        return types;
    }
    public static IEnumerable<string> GetExtendingTypeNames(this Type type)
    {
        var types = new List<string>();
        Typing.ForEachType(type, (next) =>
        {
            types.Add(next.GetTypeName());
        });
        return types;
    }
    public static string GetExtendingPath(this Type type)
    {
        string path = "";
        Typing.ForEachType(type, (next) =>
        {
            path+="=>"+(next.GetTypeName());
        });
        return path;
    }
}

/// <summary>
/// 
/// </summary>
public class TypeDocumentation : FromAttributes
{
    public class SummaryOfProperty : FromAttributes
    {
        public string Name { get; set; }
        public string Icon { get; set; } = "home";
        public string Label { get; set; } = "";
        public string Description { get; set; } = "";
        public string HelpMessage { get; set; } = "";

        public SummaryOfProperty(Type t, string prop)=>Init(t);
        
    }



    public string TypeName { get; set; }
    public string EntityIcon { get; set; } = "home";
    public string EntityLabel { get; set; } = "";
    public string HelpMessage { get; set; } = "";
    public string ClassDescription { get; set; } = "";


    public Dictionary<string, SummaryOfProperty> PropertiesDictionary { get; set; } = new Dictionary<string, SummaryOfProperty>();
    public Dictionary<string, SummaryOfProperty> ActionDictionary { get; set; } = new Dictionary<string, SummaryOfProperty>();


    public TypeDocumentation(Type type)
    {
        Init(type);
        TypeName = type.Name;
        type.GetOwnPropertyNames().ToList().ForEach(name =>
            PropertiesDictionary[name] = new SummaryOfProperty(type, name)
        );
        type.GetOwnMethodNames().ToList().ForEach(name =>
            ActionDictionary[name] = new SummaryOfProperty(type, name)
        );
    }
}