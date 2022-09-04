using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Расширения для валидации данных
/// </summary>
public static class ObjectValidateExtensions
{

    /// <summary>
    /// Выборочная проверка свойств
    /// </summary>   
    public static Dictionary<string, List<string>> ValidateProperties(this object target, string[] keys)
    {

        Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
        foreach (string key in keys)
        {
            List<string> errors = target.ValidateProperty(key);
            if (errors.Count() > 0)
            {
                results[key] = errors;
            }
        }

        return results;
    }

    /// <summary>
    /// Выборочная проверка одного свойств
    /// </summary>   
    public static List<string> ValidateProperty(this object target, string key)
    {

        List<string> errors = new List<string>();
        var attributes = Utils.ForProperty(target.GetType(), key);

        foreach (var data in target.GetType().GetProperty(key).GetCustomAttributesData())
        {
            if (data.AttributeType.GetInterfaces().Contains(typeof(MyValidation)))
            {
                List<object> args = new List<object>();
                foreach (var a in data.ConstructorArguments)
                {
                    args.Add(a.Value);
                }
                MyValidation validation =
                    ReflectionService.Create<MyValidation>(data.AttributeType, args.ToArray());
                object value = new ReflectionService().GetValue(target, key);
                string validationResult =
                    validation.Validate(target, key, value);
                if (validationResult != null)
                {

                    errors.Add(validationResult);
                }
            }
        }
        return errors;
    }



    /// <summary>
    /// Проверка данных порождает исключение при не соответвии требованиям
    /// </summary>
    public static void EnsureIsValide(this object target)
    {
        var r = target.Validate();
        if (r.Count() > 0)
        {
            string message = "";
            foreach (var p in r)
            {
                string propertyErrorsText = "";
                p.Value.ForEach((e) => { propertyErrorsText += e + ", "; });
                message += $"\t{p.Key}={propertyErrorsText}\n";
            }
            throw new ValidationException($"Обьект " + target.GetType().Name +
                " содержит не корректные данные: \n" +
                message);
        }
    }

    /// <summary>
    /// Вылидация в контексте MVC
    /// </summary> 
    public static IEnumerable<ValidationResult> Validate(this object target, ValidationContext validationContext)
    {

        List<ValidationResult> results = new List<ValidationResult>();
        Dictionary<string, List<string>> errors = target.Validate();
        foreach (var errorEntry in errors)
        {
            string propertyName = errorEntry.Key;
            List<string> propertyErrors = errorEntry.Value;
            foreach (string propertyError in propertyErrors)
            {
                ValidationResult result = new ValidationResult(propertyError, new List<string>() { propertyName });
                results.Add(result);
            }
        }
        return results;
    }

    /// <summary>
    /// Валидация модели по правилам определённым через атрибуты
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, List<string>> Validate( this object target )
    {
        
        Dictionary<string, List<string>> ModelState = new Dictionary<string, List<string>>();
        foreach (var property in target.GetType().GetProperties())
        {
            string key = property.Name;

            if (Typing.IsPrimitive(property.PropertyType))
            {
                List<string> errors = target.ValidateProperty(key);
                if (errors.Count > 0)
                {
                    ModelState[key] = errors;
                }

            }
        }
        var optional = target.ValidateOptional();
        foreach (var p in optional)
        {
            if (ModelState.ContainsKey(p.Key))
            {
                ModelState[p.Key].AddRange(optional[p.Key]);
            }
            else
            {
                ModelState[p.Key] = optional[p.Key];
            }
        }


        return ModelState;
    }


    public static Dictionary<string, List<string>> ValidateOptional(this object target )
    {
        return new Dictionary<string, List<string>>();
    }

}

