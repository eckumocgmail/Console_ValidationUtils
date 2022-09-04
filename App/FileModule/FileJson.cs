using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

  
public class FileJson
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="System.NullReferenceException"></exception>
    public static JToken Get(string filename, string selector)
    {            
        string jsontext = System.IO.File.ReadAllText(filename);
        JToken json = JsonConvert.DeserializeObject<JObject>(jsontext);
        if ( String.IsNullOrEmpty(selector))
        {
            return json;
        }
        else
        {                
            foreach (string id in selector.Split("."))
            {
                json = json[id];
                if(json == null)
                {
                    throw new System.NullReferenceException("Selection return null reference.");
                }
            }
            return json;
        }                        
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="System.NullReferenceException"></exception>
    public async static Task<JToken> GetAsync(string filename, string selector)
    {
        string jsontext = await System.IO.File.ReadAllTextAsync(filename);
        JToken json = JsonConvert.DeserializeObject<JObject>(jsontext);
        if (String.IsNullOrEmpty(selector))
        {
            return json;
        }
        else
        {
            foreach (string id in selector.Split("."))
            {
                json = json[id];
                if (json == null)
                {
                    throw new System.NullReferenceException("Selection return null reference.");
                }
            }
            return json;
        }
    }

   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="selector"></param>
    /// <param name="value"></param>
    public static void Set(string filename, string selector, string value)
    {
        string jsontext = System.IO.File.ReadAllText(filename);
        JToken root = JObject.FromObject(JsonConvert.DeserializeObject<object>(jsontext));
        
        JToken json = root;
        string[] ids = selector.Split(".");
        for( int i=0; i<(ids.Length-1); i++ )
        {
            json = json[ids[i]];
        }
        json[ids[ids.Length-1]] = value;
        System.IO.File.WriteAllText(filename, root.ToString());            
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="selector"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public async static Task SetAsync(string filename, string selector, string value)
    {
        string jsontext = await System.IO.File.ReadAllTextAsync(filename);
        JToken root = JObject.FromObject(JsonConvert.DeserializeObject<object>(jsontext));

        JToken json = root;
        string[] ids = selector.Split(".");
        for (int i = 0; i < (ids.Length - 1); i++)
        {
            json = json[ids[i]];
        }
        json[ids[ids.Length - 1]] = value;
        await System.IO.File.WriteAllTextAsync(filename, root.ToString());
    }

  
}

