 

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReCaptcha
{

    /// <summary>
    /// Служба выполняет запрос валидации формы к сервису ReCaptcha
    /// </summary>
    public class ReCaptchaService
    {
        private readonly ReCaptchaOptions _options;

        /// <summary>
        /// Служба выполняет запрос валидации формы к сервису ReCaptcha
        /// </summary>
        /// <param name="options">Параметры работы сервиса</param>
        public ReCaptchaService(ReCaptchaOptions options)
        {
            _options = options;
        }

        
        /// <summary>
        /// Выполнение запроса валидации
        /// </summary>
        /// <param name="EncodedResponse">ключ полученный с формы</param>
        /// <returns>true, если проверка пройдена успешно</returns>
        public bool Validate(string EncodedResponse)
        {
            var client = new System.Net.WebClient();            
            string PrivateKey = _options.PrivateKey;
            var reply = client.DownloadString(
                string.Format(
                    "https://www.google.com/recaptcha/api/siteverify?"+
                    "secret={0}&response={1}",
                    PrivateKey, EncodedResponse
                )
            );
            ReCaptchaResponse captchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(reply);
            switch (captchaResponse.Success)
            {
                case "false":
                    return false;
                case "true":
                    return true;
                default: throw new Exception("Странное сообщение получено в ответ от сервиса ReCaptcha");
            }
            
        }
    }
}
