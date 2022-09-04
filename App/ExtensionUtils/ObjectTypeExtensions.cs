using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Расширения объекта через тип
/// </summary>
public static class ObjectTypeExtensions
{

    public static bool IsPrimitiveType( this object target)
    {
        return Typing.IsPrimitive(target.GetType());
    }
    public static IEnumerable<string> GetOwnMethodNames( this object target) => 
        target == null? throw new ArgumentNullException(nameof(target)):
        target.GetType().GetOwnMethodNames();
    public static IEnumerable<string> GetOwnPropertyNames( this object target) =>
        target == null ? throw new ArgumentNullException(nameof(target)) : 
        target.GetType().GetOwnPropertyNames();

}



