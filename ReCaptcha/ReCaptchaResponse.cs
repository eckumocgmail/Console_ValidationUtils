using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReCaptcha
{
    /// <summary>
    /// Модель сообщений предоставляемых сервисом ReCaptcha
    /// </summary>
    public class ReCaptchaResponse
    {
        /// <summary>
        /// Статус валидации
        /// </summary>
        [JsonProperty("success")]
        public string Success
        {
            get
            {
                return m_Success;
            }
            set
            {
                m_Success = value;
            }
        }
        private string m_Success;


        /// <summary>
        /// Коды сообщений с ошибками
        /// </summary>
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get
            {
                return m_ErrorCodes;
            }
            set
            {
                m_ErrorCodes = value;
            }
        }
        private List<string> m_ErrorCodes;
    }
}
