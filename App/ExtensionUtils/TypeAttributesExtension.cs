using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Расширения для доступа к атрибутам
/// </summary>
public static class TypeAttributesExtension
{

    public static IDictionary<string,string> GetInputTypes(this Type type )
    {
        IDictionary<string, string> result = new Dictionary<string, string>();
        foreach(string property in type.GetOwnPropertyNames())
        {
            result[property] = type.GetInputType(property);
        }
        return result;
    }
    public static string GetInputType(this Type type, string property)
    {
        return Utils.GetInputType(Utils.ForProperty(type, property));
    }
    public static string Label(this Type type)
    {
        return Utils.LabelFor(type);
    }
    public static string Label(this Type type, string property)
    {
        return Utils.LabelFor(type, property);
    }    
    public static string Description(this Type type)
    {
        return Utils.DescriptionFor(type);
    }
    public static string Icon(this Type type)
    {
        return Utils.IconFor(type);
    }
    public static bool IsEntity(this Type type)
    {
        return Typing.IsExtendedFrom(type, "BaseEntity");
    }
}