using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ExceptionExtensions
{
    public static IDictionary<string, string> ToDictionaryOfText(this object target)
        => DeserializeObject<Dictionary<string, string>>(SerializeObject(target));


    public static string ToJson(this object target)
        => SerializeObject(target);

    public static IDictionary<string, string> DeserializeObject<T>(string p)
        => System.Text.Json.JsonSerializer.Deserialize<IDictionary<string, string>>(p);
    public static string SerializeObject(object target)
        => System.Text.Json.JsonSerializer.Serialize(target);
    public static string ToDocument(this Exception target)
    {
        var messages = new List<object>();
        Exception p = target;
        do
        {
            messages.Add(new
            {
                text = p.Message,
                stack = GetStack(p.StackTrace)
            });
            p = p.InnerException;
        } while (p != null);
        return messages.ToJsonOnScreen();
    }

    private static IEnumerable<object> GetStack(string stackTrace)
    {

        var list = new List<object>();
        foreach(string line in stackTrace.Split("\n"))
        {
            int i = line.IndexOf(":") - 1;
            list.Add(new
            {

                path = "file:///"+line.Substring(i, line.IndexOf(":line")-i).ReplaceAll(@"\","/"),
                line = line.Substring(line.IndexOf(":line")+ ":line".Length).Trim()
            });
        }
        return list;
    }
}


