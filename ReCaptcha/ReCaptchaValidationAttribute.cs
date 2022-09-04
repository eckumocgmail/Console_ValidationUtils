 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReCaptcha
{
    /// <summary>
    /// Атрибут валидации формы через сервис ReCaptcha
    /// </summary>
    public class RecaptchaValidationAttribute : ValidationAttribute
    {

        /// <summary>
        /// Конструктор, в качестве аргумента принимает текст сообщения с ошибкой валидации
        /// </summary>
        /// <param name="message">текст сообщения с ошибкой валидации</param>
        public RecaptchaValidationAttribute(string message): base()
        {
            ErrorMessage = message;
        }


        /// <summary>
        /// Валидация свойства
        /// </summary>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            string key = value == null ? "" : value.ToString();
            object? p = validationContext.GetService(typeof(ReCaptchaService));

            ReCaptchaService service = (p != null) ? ((ReCaptchaService)p) : null;
                
            if (service.Validate(key))
            {
                return ValidationResult.Success;
            }
            else
            {                
                var result = new ValidationResult(ErrorMessage);                
                return result;
            }            
        }
    }
}