using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

using System.Reflection;
using System.Threading.Tasks;

public class MyValidatableObject<T>: MyValidatableObject
{

}

public class MyValidatableObject: IValidatableObject, IDisposable, IAsyncDisposable
{

    [Key]
    public virtual int ID { get; set; }

    [NotMapped]
    private ILogger logger = Factory.GetLogger<MyValidatableObject>();

    public void Info(params object[] parameters) {
        foreach (var parameter in parameters)
        {
            Console.WriteLine(parameter);
        }
    }


    public void Warn(params object[] parameters)
    {
        foreach (var parameter in parameters)
        {
            Console.WriteLine(parameter);
        }
    }

    public void Error(params object[] parameters)
    {
        foreach (var parameter in parameters)
        {
            Console.WriteLine(parameter);
        }
    }

    [JsonIgnore]
    [NotMapped()]
    [NotInput()]
    public Dictionary<string, List<string>> ModelState { get; set; }


    public string GetValidationState(string Property)
    {
        return ModelState == null || ModelState.ContainsKey(Property) == false ? "valid" : "invalid";
    }



    /// <summary>
    /// Валидация модели по правилам определённым через атрибуты
    /// </summary>
    /// <returns></returns>
    public virtual Dictionary<string, List<string>> Validate()
    {
        object target = this;
        ModelState = new Dictionary<string, List<string>>();
        foreach (var property in target.GetType().GetProperties())
        {
            string key = property.Name;

            if (Typing.IsPrimitive(property.PropertyType))
            {
                List<string> errors = Validate(key);
                if (errors.Count > 0)
                {
                    ModelState[key] = errors;
                }

            }
        }
        var optional = ValidateOptional();
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

    /// <summary>
    /// true, если тип обьекта наследуется от заданного
    /// </summary> 
    public bool IsExtendedFrom(string baseType)
    {
        Type typeOfObject = new object().GetType();
        Type p = GetType();
        while (p != typeOfObject)
        {
            if (p.Name == baseType)
            {
                return true;
            }
            p = p.BaseType;
        }
        return false;
    }


    /// <summary>
    /// Выборочная проверка свойств
    /// </summary>   
    public Dictionary<string, List<string>> Validate(string[] keys)
    {
        Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
        foreach(string key in keys)
        {
            List<string> errors = Validate(key);
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
    public List<string> Validate(string key)
    {
        
        List<string> errors = new List<string>();
        var attributes = Utils.ForProperty(this.GetType(), key);

        foreach (var data in this.GetType().GetProperty(key).GetCustomAttributesData())
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
                object value = new ReflectionService().GetValue(this, key);
                string validationResult =
                    validation.Validate(this, key, value);
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
    public bool IsValide()
    {
        var r = Validate();
        if (r.Count() > 0)
            return false;
        return true;
    }

    /// <summary>
    /// Проверка данных порождает исключение при не соответвии требованиям
    /// </summary>
    public void EnsureIsValide()
    {
        var r = Validate();
        if(r.Count() > 0)
        {
            string message = "";
            foreach(var p in r)
            {
                string propertyErrorsText = "";
                p.Value.ForEach((e) => { propertyErrorsText += e + ", "; });
                message += $"\t{p.Key}={propertyErrorsText}\n";
            }
            throw new ValidationException($"Обьект "+GetType().Name + 
                " содержит не корректные данные: \n"+
                message);
        }
    }


    /// <summary>
    /// Вылидация в контексте MVC
    /// </summary> 
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        
        List<ValidationResult> results = new List<ValidationResult>();
        Dictionary<string, List<string>> errors = Validate();
        foreach(var errorEntry in errors)
        {
            string propertyName = errorEntry.Key;
            List<string> propertyErrors = errorEntry.Value;
            foreach(string propertyError in propertyErrors)
            {
                ValidationResult result = new ValidationResult(propertyError, new List<string>() { propertyName });               
                results.Add(result);
            }
        }        
        return results;
    }

    public virtual object GetValue(string key)
    {
        return ReflectionService.GetValueFor(this,key);
    }

    public virtual void SetValue(string key, object value)
    {
        PropertyInfo prop = this.GetType().GetProperty(key);
        if (prop != null)
        {
            prop.SetValue(this, value);
        }
        FieldInfo field = this.GetType().GetField(key);
        if (field != null)
        {
            field.SetValue(this, value);
        }
    }




    public virtual Dictionary<string, List<string>> ValidateOptional()
    {
        return new Dictionary<string, List<string>>();
    }

    public async ValueTask DisposeAsync()
    {
        await Task.CompletedTask;
    }

    public virtual void Dispose()
    {
       
    }
}