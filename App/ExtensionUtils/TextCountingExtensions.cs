using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Склонение по численному признаку
/// </summary>
public static class TextCountingExtensions
{

    /// <summary>
    /// Возвращает существительное во множественном числе
    /// </summary>   
    public static bool IsMultiCount(this string name)
    {
        return Counting.GetMultiCountName(name) == name;
    }

    /// <summary>
    /// Возвращает существительное в ед. числе
    /// </summary>   
    public static bool IsSingleCount(this string name)
    {
        return Counting.GetSingleCountName(name) == name;
    }
    public static bool IsTsqlStyled(this string name)
    {
        throw new NotImplementedException();
    }
    

    
    /// <summary>
    /// Возвращает существительное во множественном числе
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string ToMultiCount(this string name)
    {
        return Counting.GetMultiCountName(name);
    }


    /// <summary>
    /// Возвращает существительное в единственном
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string ToSingleCount(this string name)
    {
        return Counting.GetSingleCountName(name);
    }

}
