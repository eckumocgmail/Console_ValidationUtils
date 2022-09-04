

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AppHttpClient
{

    /// <summary>
    /// Клиент контроллера помеченного атрибутов [Route("[controller]/[action]")]
    /// </summary>
    public class ControllerHttpClient: Logging 
    {
        /*public static int Counter = StartupApplication.Add((services) => {
            
            services.AddTransient(typeof(AuthorizationHttpClient), (provider) => {
                return new AuthorizationHttpClient(provider.GetRequiredService<ITokenStorage>(), "");
            });
        });*/

        public bool _logging { get; set; }

        private readonly ITokenStorage _storage;
        private readonly Logging _logger;
        private readonly HttpClient _http;
        public readonly string _url;

        private readonly IDictionary<string, string> _cookies =
            new Dictionary<string, string>();


        /// <summary>
        /// Конструктор http-клиента настроенного на работу с контроллером
        /// </summary>
        /// <param name="url">URL адрес контроллера помеченного атрибутов [Route("[controller]/[action]")]</param>
        public ControllerHttpClient( ITokenStorage storage,string url, bool logging=true )
        {
            _storage = storage;
            _logger = new Logger(GetType().Name, GetHashCode());
            _http = new HttpClient();            
            _url = url;
            _logging = logging;
        }
        
        
        public async Task<string> GetToken()
        {
           
            return await _storage.Get();
                    
        }
        public async Task SetToken(string value)
        {           
            await _storage.Set(value);                         
        }


        /// <summary>
        /// Выполнение запроса методом POST
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения, передаваемого в теле запроса</typeparam>
        /// <param name="action">имя операции</param>
        /// <param name="Message">данные для запроса</param>
        /// <returns>текст тела сообщения</returns>
        public async Task<string> Post<TMessage,TResult>(string action,  TMessage Message)
        {
            string url = _url + "/" + action;
            if (_logging) Info($"[POST][{url}]");
            string token = await GetToken();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("HTTP", token);
            string RequestBody = JObject.FromObject(Message).ToString();
            request.Content = new StringContent(RequestBody, Encoding.UTF8, "text/plain");   
            HttpResponseMessage httpResponseMessage = await _http.SendAsync(request);                       
            return await LogHttpResponse(url, httpResponseMessage);
        }

        /// <summary>
        /// Выполнение запроса методом POST
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения, передаваемого в теле запроса</typeparam>
        /// <param name="action">имя операции</param>
        /// <param name="Headers">заголовки запроса</param>
        /// <param name="Message">данные для запроса</param>
        /// <returns>текст тела сообщения</returns>
        public async Task<TMessage> Post<TMessage>(string action, 
            IDictionary<string, string> Headers,
            IDictionary<string, string> Parameters)
        {
            string url = _url + "/" + action;
            if (_logging) Info($"[POST][{url}]");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            bool firstParameter = true;
            foreach (var kv in Parameters)
            {
                if (firstParameter)
                {
                    url += $"?{kv.Key}={kv.Value}";
                    firstParameter = false;
                }
                else
                {
                    url += $"&{kv.Key}={kv.Value}";
                }
            }
            //string token = await _storage.Get();
            //request.Headers.Authorization = 
            //    new System.Net.Http.Headers.AuthenticationHeaderValue("HTTP", token);
            //request.Headers.Add("Cookie", "UserKey=" + token);
            string RequestBody = JObject.FromObject(Parameters).ToString();
            request.Content = new StringContent(RequestBody, Encoding.UTF8, "text/plain");
            HttpResponseMessage httpResponseMessage = await _http.SendAsync(request);            
            string responseText = await LogHttpResponse(url, httpResponseMessage);
            if (Typing.IsPrimitive(typeof(TMessage)))
            {
                return (TMessage)TextDataSetter.FromText(responseText, typeof(TMessage).Name);
            }
            else
            {
                return responseText.FromJson<TMessage>();
            }
        }


        /// <summary>
        /// Выполнение запроса методом GET
        /// </summary>
        /// <param name="action">имя операции</param>
        /// <returns>текст из тела ответного сообщения</returns>
        /*public async Task<string> Get(string action)
        {
            string url = _url + "/" + action;
            if (_logging) Info($"[GET][{url}]");

            var response = await _http.GetAsync(url);
            string Body = await LogHttpResponse (url, response);
            return Body;
        }*/

        /// <summary>
        /// Выполнение запроса методом GET
        /// </summary>
        /// <param name="action">имя операции</param>
        /// <returns>текст из тела ответного сообщения</returns>
        public async Task<TResponse> Get<TResponse>(string action)
        {
            string token = await _storage.Get();
            string key = token;
            var headers = new Dictionary<string, string>() 
            {
                { "Authorization",key },
                { "Cookie","UserKey="+key },
            };
            
            return await Get<TResponse>(action, headers);
        }
        public async Task<TResponse> Get<TResponse>(string action, Dictionary<string,string> Headers)
        {
            string url = _url + "/" + action;
      
            if (_logging) Info($"[POST][{url}]");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            foreach (var kv in Headers)
            {
                
                request.Headers.Add(kv.Key, kv.Value);
            }
            string token = await _storage.Get();
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("HTTP", token);
            request.Headers.Add("Cookie", "UserKey=" + token);
 
            request.Content = new StringContent(null, Encoding.UTF8, "text/plain");
            HttpResponseMessage httpResponseMessage = await _http.SendAsync(request);
            string body = await LogHttpResponse(url, httpResponseMessage);
            return body.FromJson<TResponse>();

            /*var response = await _http.GetAsync(url);
            string Body = await LogHttpResponse(url, response);

            return Body.Deseriallize<TResponse>();*/
        }

        public async Task<TResponse> Get<TResponse>(string action,
         
            IDictionary<string, string> parameters)
        {
            string url = _url + "/" + action;
            bool firstParameter = true;
            foreach (var kv in parameters)
            {
                if (firstParameter)
                {
                    url += $"?{kv.Key}={kv.Value}";
                    firstParameter = false;
                }
                else
                {
                    url += $"&{kv.Key}={kv.Value}";
                }
            }


            if (_logging) Info($"[GET][{url}]");
            _http.DefaultRequestHeaders.Add("Cookie", "");
            var response = await _http.GetAsync(url);
            string responseText = await LogHttpResponse(url, response);
 
            if (Typing.IsPrimitive(typeof(TResponse).Name))
            {
                return (TResponse)TextDataSetter.FromText(responseText, typeof(TResponse).Name);
            }
            else
            {
                return responseText.FromJson<TResponse>();
            }
           
            /*
            string url = _url + "/" + action;
            bool firstParameter = true;
            foreach (var kv in parameters)
            {
                if (firstParameter)
                {
                    url += $"?{kv.Key}={kv.Value}";
                    firstParameter = false;
                }
                else
                {
                    url += $"&{kv.Key}={kv.Value}";
                }
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            string token = await GetToken();
            if (string.IsNullOrEmpty(token) == false)
            {
                Headers["Authorization"] = token;
                request.Headers.Add("Cookie", "UserKey=" + token);
            }
            
            //request.Headers.Authorization =
                //new System.Net.Http.Headers.AuthenticationHeaderValue("HTTP", token);            
            //request.Content = new StringContent(new { }.ToJson(), Encoding.UTF8, "text/plain");
            HttpResponseMessage httpResponseMessage = await _http.SendAsync(request);
            string responseText = await LogHttpResponse(action, httpResponseMessage);
            */

            
        }
        public class CookieModel
        {
            public string UserKey { get; set; }
            public string Path { get; set; }
        }

        /// <summary>
        /// Логированние ответного сообщения включая заголовки
        /// </summary>
        /// <param name="action">Имя операции</param>
        /// <param name="response">Ссылка на сообщение</param>
        /// <returns>текст тела сообщения</returns>
        private async Task<string> LogHttpResponse(string action, HttpResponseMessage response)
        {
            if (_logging) Info($"[GET][{action}][{response.StatusCode}]");
            var enumerator = response.Headers.GetEnumerator();
            if (_logging) Info($"Headers:");
            while (enumerator.MoveNext())
            {
                if (_logging)
                {
                    string values = "[";
                    enumerator.Current.Value.ToList().ForEach(value =>
                    {
                        values += value + ",";
                    });
                    values = values.Substring(0, values.Length - 1) + "]";
                    Info($" \t {enumerator.Current.Key}={values}");
                }
                if(enumerator.Current.Key == "Set-Cookie")
                {
                    var values =  enumerator.Current.Value.ToArray();
                    for(int i=0; i< values.Length; i++)
                    {
                        var value = values[i];


                        var model = new CookieModel();
                        IDictionary<string,string> parameters = ParseParameters(value.ToString());
                        if (parameters.ContainsKey("UserKey") == false)
                        {
                            throw new Exception("Не удалось получить параметр UserKey");
                        }
                        else
                        {
                            model.UserKey = parameters["UserKey"];
                        }
                        if (parameters.ContainsKey("path") == false)
                        {
                            throw new Exception("Не удалось получить параметр UserKey");
                        }
                        else
                        {
                            model.Path = parameters["path"];
                        }


                        this._cookies[model.Path] = model.UserKey;
                      
                        string token = model.UserKey;
                        await _storage.Set(token);
                        Info("Сохраняем куки в локальное хранилище "+ model.UserKey);
                    };
                }
            }
            string Body = await response.Content.ReadAsStringAsync();
            if (_logging) Info($" Body:");
            if (_logging) Info($"{Body}");
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.NoContent:
                    return "";
                case System.Net.HttpStatusCode.OK:
                    return Body;

                case System.Net.HttpStatusCode.InternalServerError:
                    throw new Exception(Body);
                default:
                    throw new NotImplementedException("Обработка статуса "+ response.StatusCode.ToString()+" в настоящий момент не выполняется");
            }
            
        }

        private IDictionary<string,string> ParseParameters(string message)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (string text in message.Split(";"))
            {
                string[] items = text.Trim().Split("=");
                if (items.Length != 2)
                {
                    throw new Exception("Не удалось разобрать текст как параметры");
                }
                parameters[items[0].Trim()] = items[1].Trim();
            }
            return parameters;
        }

        public void Info(string message)
        {

            
            _logger.Info(message);
        }

      
    }
    public interface Logging
    {
        public void Info(string message);
    }


    /// <summary>
    /// Методы логирования в консоль
    /// </summary>
    class Logger: Logging
    {
        private readonly string _type;
        private readonly int _hash;

        public Logger(string type, int hash)
        {
            _type = type;
            _hash = hash;
        }

        public void Info(string message)
        {
            System.Console.WriteLine($"\n[INFO][{_type}][{_hash}] => ");
            System.Console.WriteLine($"{message}\n");
            
        }
    }
}
