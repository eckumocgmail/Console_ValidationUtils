using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Методы расширения
/// </summary>
[EntityIcon("")]
[EntityLabel("")]
[Description("Контроллер предназначен для .")]
public static class AppRootExtensions
{

    /// <summary>
    /// 
    /// </summary>
    public class ExceptionCatcherMiddleware 
    {
        protected Func<Exception, string> _Catch;
        public ExceptionCatcherMiddleware(Func<Exception, string> Catch)
        {
            _Catch = Catch;
        }
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                Console.WriteLine($"{context.Items} ");
                return next.Invoke(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("catch");
                return Task.Run(() => {
                    context.Response.WriteAsync(_Catch(ex));
                });
            }
        }
    }

    
    
    public static T TryGet<T>(this IDictionary<string, string> data, string key) where T: class
    {
        if (data.ContainsKey(key))
        {

            if (Typing.IsPrimitive(typeof(T)) == true)
            {
                return (T)Setter.FromText(data[key],typeof(T).GetTypeName());
            }
            else
            {
                return data[key].FromJson<T>();
            }
        }
        else
        {
            return null;
        }
        
    }
    public static IDictionary<string, string> GetRoutes(this HttpContext httpContext, params string[] keys) => httpContext.GetFromQuery(keys);
    public static IDictionary<string, string> GetFromQuery(this HttpContext httpContext, params string[] keys)
    {
        var result = new Dictionary<string, string>();

        var names = httpContext.Request.Path.ToString().Substring(1).Split("/");        
        for(int i=0; i < keys.Length; i++)
        {
            result[keys[i]] = i< names.Length? names[i]: null;
        }
        return result;
    }




    /// <summary>
    /// Извлечение модели запроса
    /// </summary>
    public static DataRequestMessage ToRequestMessage(this HttpContext httpContext)
    {
        global::LoggerFactory.Create(options => options.AddConsole()).CreateLogger(
            nameof(AppRootExtensions) + "." +
            nameof(ToRequestMessage)
        ).LogInformation($"ToRequestMessage( )");

        DataRequestMessage Request = new DataRequestMessage();
        var headers = new Dictionary<string, string>();
        foreach (var kv in httpContext.Request.Headers)
            headers[kv.Key] = kv.Value;

        Request.Headers = headers;
        Request.TraceId = httpContext.TraceIdentifier;
        Request.ActionName = httpContext.Request.Path.Value.ToString();
        Request.ParametersMap = httpContext.GetQueryParams();
        Request.MessageBody = httpContext.ReadRequestBody().Result;
        return Request;
    }



    /// <summary>
    /// Модель "Удалённый вызов процедуры"
    /// </summary>
    [EntityIcon("")]
    [EntityLabel("")]
    [Description("Контроллер предназначен для .")]
    public class DataRequestMessage
    {


        /// <summary>
        /// Ключ доступа клиента
        /// </summary>
        [JsonProperty("token")]
        public string AccessToken { get; set; }


        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [JsonProperty("mid")]
        public string TraceId { get; set; }

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [JsonProperty("pk")]
        public string SerialKey { get; set; }


        /// <summary>
        /// Имя процедуры
        /// </summary>
        [JsonProperty("request.path")]
        public string ActionName { get; set; }


        /// <summary>
        /// Параметры выполнения
        /// </summary>
        [JsonProperty("request.pars")]
        public Dictionary<string, object> ParametersMap { get; set; }


        public byte[] MessageBody { get; set; }
        public Dictionary<string, string> Headers { get; set; }




    }
    /// <summary>
    /// Метод сериализации парамтеров в строку запроса
    /// </summary>
    public static string ToQueryString(this Dictionary<string, object> Params)
    {
        global::LoggerFactory.Create(options => options.AddConsole()).CreateLogger(
            nameof(AppRootExtensions) + "." +
            nameof(GetQueryParams)
        ).LogInformation($"ToQueryString(${Params.ToJsonOnScreen()})");

        string QueryString = "";
        foreach (var Entry in Params)
        {
            Type Type = Entry.GetType();
            QueryString += $"{Entry.Key}={Entry.Value}&";
        }
        return QueryString.Length > 0 ? "?" + QueryString : "";
    }


    /// <summary>
    /// Извлечение параметров запроса и контекста
    /// </summary>
    /// <param name="Http"> контекст протокола </param>
    /// <returns></returns>
    public static Dictionary<string, object> GetQueryParams(this HttpContext Http)
    {
        global::LoggerFactory.Create(options => options.AddConsole()).CreateLogger(
            nameof(AppRootExtensions) + "." +
            nameof(GetQueryParams)
        ).LogInformation($"GetQueryParams()");

        Dictionary<string, object> pars = new Dictionary<string, object>();
        foreach (var Entry in Http.Request.Query)
        {
            pars[Entry.Key] = Entry.Value;
        }
        return pars;
    }


    /// <summary>
    /// Считывание бинарных данных в основном блоке сообщения
    /// </summary>
    public static async Task<byte[]> ReadRequestBody(this HttpContext httpContext)
    {
        global::LoggerFactory.Create(options => options.AddConsole()).CreateLogger(
            nameof(AppRootExtensions) + "." +
            nameof(ReadRequestBody)
        ).LogInformation($"ReadRequestBody()");

        long? length = httpContext.Request.ContentLength;
        if (length != null)
        {
            byte[] data = new byte[(long)length];
            await httpContext.Request.Body.ReadAsync(data, 0, (int)length);
            string mime = httpContext.Request.ContentType;
            return data;
        }
        return new byte[0];
    }
}