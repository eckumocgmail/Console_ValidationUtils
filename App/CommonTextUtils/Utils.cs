
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using System.Threading.Tasks;

public class Utils
{
    public static ConcurrentDictionary<string, Dictionary<string, string>> AttrsByType = new ConcurrentDictionary<string, Dictionary<string, string>>();
    public static Dictionary<string, Dictionary<string, Dictionary<string, string>>> AttrsByMembers = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
    public static bool IsUniq(Type target, string name)
    {
        throw new NotImplementedException();
    }
    public static string IconFor(string type)
    {
        return IconFor(ReflectionService.TypeForName(type));
    }

    public static bool HasAttribute<T>(Dictionary<string, string> attrs) where T : Attribute
    {
        string typeName = typeof(T).GetTypeName();
        return attrs.ContainsKey(typeName) || attrs.ContainsKey(typeName.Replace("Attribute", ""));
    }

    /// <summary>
    /// Проверка флага отображением
    /// </summary>
    /// <param name="type"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static bool IsVisible(Type type, string property)
    {
        string hidden = ForPropertyValue(type, typeof(InputHiddenAttribute), property);
        return "True" == hidden ? false : true;
    }


    /// <summary>
    /// Получить значения атрибуf заданного для свойства
    /// </summary>
    /// <param name="type"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static string ForPropertyValue(Type type, Type attr, String property)
    {
        if (type == null)
        {
            throw new Exception("Аргумент " + type + " содержить ссылку на null");
        }
        Dictionary<string, string> attrs = new Dictionary<string, string>();
        if (type == null || type.GetProperty(property) == null)
        {
            throw new Exception("Свойство не найдено либо не задан тип");
        }
        foreach (var data in type.GetProperty(property).CustomAttributes)
        {
            string key = data.AttributeType.Name;
            if (key == attr.Name)
            {
                foreach (var arg in data.ConstructorArguments)
                {
                    string value = arg.Value.ToString();
                    return value;
                }
            }


        }
        return null;
    }


    /// <summary>
    /// Получение атрибута типа поля ввода
    /// </summary>
    /// <param name="attrs"></param>
    /// <returns></returns>
    public static string GetInputType(Dictionary<string, string> attrs)
    {
        if (attrs.ContainsKey("Key") || attrs.ContainsKey("KeyAttribute"))
        {
            return "hidden";
        }
        string key = null;
        List<string> keys = new List<string>(attrs.Keys);
        InputTypeAttribute.GetInputTypes().ForEach((string name) =>
        {
            if (keys.Contains(name))
            {
                key = name;
            }
        });
        if (key != null)
        {
            return key.Replace("Attribute", "").Replace("Input", "");
        }
        else
        {

            return null;
        }
    }




    /// <summary>
    /// Подпись элемента визуализации ассоциированного со заданным свойством 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetInputType(Type type, string name)
    {
        Dictionary<string, string> attrs = ForProperty(type, name);
        return GetInputType(attrs);
    }

    public static string IconFor(Type type)
    {
        Dictionary<string, string> attrs = ForType(type);
        return attrs.ContainsKey(nameof(IconAttribute)) ? attrs[nameof(IconAttribute)] :
            attrs.ContainsKey(nameof(EntityIconAttribute)) ? attrs[nameof(EntityIconAttribute)] :
            null;
    }
    public static Dictionary<string, string> ForMethod(Type controllerType, string name)
    {
        Dictionary<string, string> attrs = new Dictionary<string, string>();
        foreach (var method in controllerType.GetMethods())
        {
            foreach (var data in method.GetCustomAttributesData())
            {
                string key = data.AttributeType.Name;
                foreach (var arg in data.ConstructorArguments)
                {
                    string value = arg.Value.ToString();
                    attrs[key] = value;
                }

            }
        }
        return attrs;
    }

    public static Dictionary<string, Dictionary<string, string>> GetAttributesByMemberForType(Type type)
    {
        type.GetProperties().ToList().Select(p => p.Name).ToList().ForEach((name) => {
            ForProperty(type, name).ToJsonOnScreen().WriteToConsole();
        });
        return AttrsByMembers[type.Name];
    }

    public static IEnumerable<string> GetRefTypPropertiesNames(Type type)
    {
        return type.GetProperties().ToList().Where(p => Typing.IsPrimitive(p.PropertyType) == false).Select(p => p.Name);
    }

    /// <summary>
    /// Возвращаент имя свойства помеченного заданным атриюбутом
    /// </summary>
    /// <param name="target"></param>
    /// <param name="nameOfInputTypeAttribute"></param>
    /// <returns></returns>
    public static object GetValueMarkedByAttribute(object target, string nameOfInputTypeAttribute)
    {
        return ForAllPropertiesInType(target.GetType()).Where(p => p.Value.ContainsKey(nameOfInputTypeAttribute)).Select(p => p.Key).Single();

    }


    public static Dictionary<string, Dictionary<string, string>> ForAllMethodsInType(Type type)
    {
        Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
        foreach (var prop in type.GetMethods())
        {
            Dictionary<string, string> forProperty = ForMethod(type, prop.Name);
            result[prop.Name] = forProperty;
        }
        return result;
    }
    public static Dictionary<string, Dictionary<string, string>> ForAllPropertiesInType(Type type)
    {
        Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
        foreach (var prop in type.GetProperties())
        {
            Dictionary<string, string> forProperty = ForProperty(type, prop.Name);
            result[prop.Name] = forProperty;
        }
        return result;
    }
    public static Dictionary<string, Dictionary<string, string>> ForAllFieldsInType(Type type)
    {
        Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
        foreach (var prop in type.GetFields())
        {
            Dictionary<string, string> forProperty = ForField(type, prop.Name);
            result[prop.Name] = forProperty;
        }
        return result;
    }

    public static Attribute[] ForPropertyLikeAttrubtes(Type type, string property)
    {
        var attrs = new List<Attribute>();
        if (type == null)
        {
            throw new ArgumentNullException();
        }



        PropertyInfo info = type.GetProperty(property);
        if (info == null)
        {
            throw new Exception($"Свойство {property} не найдено в обьекте типа {type.Name}");
        }
        foreach (var data in info.GetCustomAttributesData())
        {
            string key = data.AttributeType.Name;

            if (data.ConstructorArguments == null || data.ConstructorArguments.Count == 0)
            {
                Attribute attr = ReflectionService.Create<Attribute>(key, new object[0]);
                attrs.Add(attr);
            }
            else
            {
                List<object> parameters = new List<object>();
                foreach (CustomAttributeTypedArgument arg in data.ConstructorArguments)
                {
                    parameters.Add(arg.Value);
                }
                Attribute attr = ReflectionService.Create<Attribute>(key, parameters.ToArray());
                attrs.Add(attr);

            }

            //model.Attributes[data.AttributeType] = null;

        }
        return attrs.ToArray();
    }

    public static List<string> SearchTermsForType(Type p)
    {
        List<string> terms = new List<string>();
        Dictionary<string, string> attrs = ForType(p);
        if (attrs.ContainsKey(nameof(SearchTermsAttribute)))
        {
            terms.AddRange(attrs[nameof(SearchTermsAttribute)].Split(","));
        }
        return terms;
    }
    public static List<string> GetSearchTerms(string entity)
    {
        Type entityType = ReflectionService.TypeForShortName(entity);
        List<string> terms = SearchTermsForType(entityType);
        return terms;
    }

    /// <summary>
    /// Подпись элемента визуализации ассоциированного со заданным свойством 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string LabelFor(Type model, string name)
    {
        Dictionary<string, string> attrs = ForProperty(model, name);
        return attrs.ContainsKey(nameof(LabelAttribute)) ? attrs[nameof(LabelAttribute)] :
            attrs.ContainsKey(nameof(DisplayAttribute)) ? attrs[nameof(DisplayAttribute)] : name;
    }


    /// <summary>
    /// Получение значения атрибута для текста надписи
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string LabelFor(Type type)
    {
        Dictionary<string, string> attrs = ForType(type);
        return attrs.ContainsKey(nameof(EntityLabelAttribute)) ? attrs[nameof(EntityLabelAttribute)] :
            attrs.ContainsKey(nameof(LabelAttribute)) ? attrs[nameof(LabelAttribute)] : null;
    }

    public static string DescriptionFor(Type type, string property)
    {
        Dictionary<string, string> attrs = ForProperty(type, property);
        return attrs.ContainsKey(nameof(ClassDescriptionAttribute)) ? attrs[nameof(ClassDescriptionAttribute)] :
            attrs.ContainsKey(nameof(DescriptionAttribute)) ? attrs[nameof(DescriptionAttribute)] : null;
    }


    /// <summary>
    /// Получение атрибутов для обьекта
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ForObject(object p)
    {

        return ForType(p.GetType());
    }


    public static bool IsInput(Type type, string name)
    {
        return IsInput(ForProperty(type, name));
    }
    public static bool IsInput(Dictionary<string, string> attrs)
    {
        return attrs.ContainsKey(nameof(NotInputAttribute)) ? false : true;
    }

    public static string LabelFor(object p)
    {
        Dictionary<string, string> attrs = ForObject(p);
        return attrs.ContainsKey(nameof(EntityLabelAttribute)) ? attrs[nameof(EntityLabelAttribute)] :
            attrs.ContainsKey(nameof(DisplayAttribute)) ? attrs[nameof(DisplayAttribute)] : p.GetType().Name;
    }

    public static string DescriptionFor(object p)
    {
        Dictionary<string, string> attrs = ForObject(p);
        return attrs.ContainsKey(nameof(DescriptionAttribute)) ? attrs[nameof(DescriptionAttribute)] :
                attrs.ContainsKey(nameof(ClassDescriptionAttribute)) ? attrs[nameof(ClassDescriptionAttribute)] : "";
    }

    public static string IconFor(Type type, string property)
    {
        Dictionary<string, string> attrs = ForProperty(type, property);
        return attrs.ContainsKey(nameof(IconAttribute)) ? attrs[nameof(IconAttribute)] : "person";

    }
    /// <summary>
    /// Получить значения всех атрибутов заданных для свойства
    /// </summary>
    /// <param name="type"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ForProperty(Type type, String property)
    {
        if (type == null)
        {
            throw new ArgumentNullException();
        }
        if (AttrsByMembers.ContainsKey(type.Name) == false)
        {
            AttrsByMembers[type.Name] = new Dictionary<string, Dictionary<string, string>>();
        }
        if (AttrsByMembers[type.Name].ContainsKey(property))
        {
            return AttrsByMembers[type.Name][property];
        }

        Dictionary<string, string> attrs =
            AttrsByMembers[type.Name][property] =
                new Dictionary<string, string>();
        PropertyInfo info = type.GetProperty(property);
        if (info == null)
        {
            throw new Exception($"Свойство {property} не найдено в обьекте типа {type.Name}");
        }
        foreach (var data in info.GetCustomAttributesData())
        {

            string key = data.AttributeType.Name;
            //ParameterInfo[] pars = data.AttributeType.GetConstructors()[0].GetParameters();
            if (data.ConstructorArguments == null || data.ConstructorArguments.Count == 0)
            {
                attrs[key] = "";
            }
            else
            {
                foreach (var arg in data.ConstructorArguments)
                {

                    string value = arg.Value == null ? "" : arg.Value.ToString();
                    attrs[key] = value;
                }
            }

            //model.Attributes[data.AttributeType] = null;

        }
        return attrs;
    }


    /// <summary>
    /// Получить значения всех атрибутов заданных для свойства
    /// </summary>
    /// <param name="type"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ForField(Type type, String property)
    {
        if (type == null)
        {
            throw new ArgumentNullException();
        }
        if (AttrsByMembers.ContainsKey(type.Name) == false)
        {
            AttrsByMembers[type.Name] = new Dictionary<string, Dictionary<string, string>>();
        }
        if (AttrsByMembers[type.Name].ContainsKey(property))
        {
            return AttrsByMembers[type.Name][property];
        }

        Dictionary<string, string> attrs =
            AttrsByMembers[type.Name][property] =
                new Dictionary<string, string>();
        var info = type.GetField(property);
        if (info == null)
        {
            throw new Exception($"Свойство {property} не найдено в обьекте типа {type.Name}");
        }
        foreach (var data in info.GetCustomAttributesData())
        {

            string key = data.AttributeType.Name;
            //ParameterInfo[] pars = data.AttributeType.GetConstructors()[0].GetParameters();
            if (data.ConstructorArguments == null || data.ConstructorArguments.Count == 0)
            {
                attrs[key] = "";
            }
            else
            {
                foreach (var arg in data.ConstructorArguments)
                {

                    string value = arg.Value == null ? "" : arg.Value.ToString();
                    attrs[key] = value;
                }
            }

            //model.Attributes[data.AttributeType] = null;

        }
        return attrs;
    }






    public static Dictionary<string, string> ForType(Type p)
    {
        if (AttrsByType.ContainsKey(p.Name))
        {
            return AttrsByType[p.Name];
        }
        else
        {
            Dictionary<string, string> attrs = new Dictionary<string, string>();
            foreach (var data in p.GetCustomAttributesData())
            {
                string key = data.AttributeType.Name;
                foreach (var arg in data.ConstructorArguments)
                {
                    string value = arg.Value.ToString();
                    attrs[key] = value;
                }

            }
            AttrsByType[p.Name] = attrs;
            return attrs;
        }
    }



    /// <summary>
    /// Извлечение метода HTTP из атрибутов
    /// </summary>
    /// <param name="attributes"></param>
    /// <returns></returns>
    public static string ParseHttpMethod(Dictionary<string, string> attributes)
    {
        foreach (var p in attributes)
        {
            switch (p.Key)
            {
                case "HttpPostAttribute":
                    return "GET";
                case "HttpPutAttribute":
                    return "PUT";
                case "HttpPatchAttribute":
                    return "PATCH";
                case "HttpDeleteAttribute":
                    return "DELETE";
                default: return "GET";
            }
        }
        return "GET";
    }
    public static bool IsUniq(Dictionary<string, string> attributes)
    {
        return attributes.ContainsKey(nameof(UniqValidationAttribute));
    }

    public static string GetUniqProperty(Dictionary<string, Dictionary<string, string>> attrs)
    {
        foreach (var p in attrs)
        {
            if (IsUniq(attrs[p.Key]))
            {
                return p.Key;
            }
        }
        return null;
    }

}