using ApplicationUnit.Encoder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Расширения для сжатия текста
/// </summary>
public static class HaffmanAlgExtensions
{

    /// <summary>
    /// Сжатие
    /// </summary>
    public static string Encode(this object target)
    {
        var enc = new CharacterEncoder();
        return enc.Encode(target.ToJson());
    }

    /// <summary>
    /// Распаковка
    /// </summary>
    public static string Decode<TTable>(this string target)
    {
        var enc = new CharacterEncoder();
        return enc.Decode(target);
    }

    /// <summary>
    /// Десериализация из Json
    /// </summary>
    public static TTable FromJson<TTable>(this string target)
    {
        return JsonConvert.DeserializeObject<TTable>(target);
        
    }
}