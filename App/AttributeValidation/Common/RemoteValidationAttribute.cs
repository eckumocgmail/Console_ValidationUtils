 
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetCoreConstructorAngular.Data.DataAttributes
{
    public class RemoteValidationAttribute:  MyValidation
    {        
        private readonly string _uri;

        private static string Parse( string uri, int number )
        {
            try
            {
                string[] ids = uri.Split("/");
                return ids[number];
            }
            catch(Exception ex)
            {
                Writing.ToConsole("Ошибка при разбре URI="+uri+": "+ex.Message);
                throw new Exception("Ошибка при разбре URI=" + uri + ": " + ex.Message, ex );
            }
        }


        public RemoteValidationAttribute(string uri)    
        {
            string controller = Parse(uri, 1);
            string action = Parse(uri, 2);
            _uri = uri;
        }

        public string Validate(object model, string property, object value)
        {
            var http = new HttpClient();
            var resp = http.GetAsync(this._uri).Result;
            resp.EnsureSuccessStatusCode();
            string responseText = resp.Content.ReadAsStringAsync().Result;
            var dictiopnary = Formating.FromJson(responseText);
            if (dictiopnary.Count() > 0)
            {
                string error = "";
                foreach(var p in dictiopnary)
                {
                    error += p.Value + "\n";
                }
                return error;
            }
            else
            {
                return null;
            }



        }

        public string GetMessage(object model, string property, object value)
        {
            return $"Не выполнена функция валидации модели {model.GetType().Name} для свойства {property}";
        }
    }
}
