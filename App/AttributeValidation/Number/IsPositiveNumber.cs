﻿using NetCoreConstructorAngular.Data.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class IsPositiveNumber : BaseValidationAttribute
{
    private readonly string _message;

    public IsPositiveNumber(string message=null)
    {
        _message = message;
    }
    public override string GetMessage(object model, string property, object value)
    {
        if (string.IsNullOrEmpty(_message))
        {
            return "Значение должно быть положительным";
        }
        else
        {
            return _message;
        }
    }

    public override string Validate(object model, string property, object value)
    {
        if (value != null)
        {            
            return value.ToString().ValidateIsPositiveInt();
        }
        else
        {
            return null;
        }
    }
}