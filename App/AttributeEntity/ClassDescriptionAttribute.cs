using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
public class LabelAttribute : DisplayNameAttribute
{
    public LabelAttribute(string text) : base(text)
    {
    }
}
public class IconAttribute : Attribute
{
    public IconAttribute(string icon)
    {
    }
}
public class HelpMessageAttribute : Attribute
{
    public HelpMessageAttribute(string message)
    {
    }
}
public class EntityLabelAttribute : DisplayNameAttribute
{
    public EntityLabelAttribute(string text) : base(text)
    {
    }
}
public class EntityIconAttribute : Attribute
{
    public EntityIconAttribute(string text)
    {
    }
}
public class DescriptionAttribute : Attribute
{
    private readonly string _message;

    public DescriptionAttribute(string message)
    {
        _message = message;
    }
}
public class ClassDescriptionAttribute : Attribute
{    
    public ClassDescriptionAttribute(string message="")
    {
    }
}
public class DetailsAttribute : Attribute
{
    public DetailsAttribute(string message = "")
    {
    }
}