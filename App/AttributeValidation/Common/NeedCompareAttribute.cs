using NetCoreConstructorAngular.Data.DataAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NeedCompareAttribute : ValidationAttribute, MyValidation
{
    private readonly string _property;
    private readonly string _message;

    public NeedCompareAttribute(string property, string message)
    {
        _property = property;
        _message = message;
    }

    public string GetMessage(object model, string property, object value)
    {
        return _message;
    }

    public string Validate(object model, string property, object value)
    {
        object value2 = model.GetType().GetProperty(_property).GetValue(model);
        if (value != null && value2!=null)
        {
            if (value.ToString() != value2.ToString())
                return GetMessage(model, property, value);
        }
        return null;
    }
}