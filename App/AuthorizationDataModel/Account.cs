
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationDb.Entities
{

    public class AccountRegistration : BaseEntity
    {


        [InputOrder(1)]
        [InputEmail("Электронный адрес задан некорректно")]
        [Label("Электронный адрес")]
        [NotNullNotEmpty("Не указан электронный адрес")]
        [Icon("email")]
        [UniqValidation("Этот адрес электронной почты уже зарегистрирован")]
        [JsonProperty("Email")]
        public string Email { get; set; } = "eckumocuk@gmail.com";


        [InputOrder(2)]
        [Label("Пароль")]
        [NotNullNotEmpty]
        [InputPassword()]
        [NotMapped]

        [JsonProperty("Password")]
        public string Password { get; set; } = "eckumocuk@gmail.com";

        [InputOrder(3)]
        [Label("Подтверждение")]
        [NotNullNotEmpty]
        [InputPassword()]
        [NotMapped]
        [Compare("Password")]

        [JsonProperty("Confirmation")]
        public string Confirmation { get; set; } = "eckumocuk@gmail.com";
    }
    /// <summary>
    /// Ученая запись пользователя
    /// </summary>
    [EntityLabel("Учетная запись")]
    [EntityIcon("account_box")]
    public class Account: BaseEntity
    {



        [InputEmail("Электронный адрес задан некорректно")]
        [Label("Электронный адрес")]
        [NotNullNotEmpty("Не указан электронный адрес")]
        [Icon("email")]
        [UniqValidation("Этот адрес электронной почты уже зарегистрирован")]
        [JsonProperty("Email")]
        public string Email { get; set; }


        [Label("Пароль")]
        [NotNullNotEmpty]
        [InputPassword()]
        [NotMapped]

        [JsonProperty("Password")]
        public string Password { get; set; }


        /// <summary>
        /// Время активации
        /// </summary>
        [AllowNull]        
        [InputDate( )]
        [InputHidden(true)]
        [NotInput("Свойство " + nameof(Activated) + " не вводится пользователем, оно устанавливается системой после ввода ключа активации")]
        public DateTime? Activated { get; set; }

        [InputHidden(true)]
        [NotInput("Свойство " + nameof(ActivationKey) + " не вводится пользователем, оно устанавливается системой перед созданием сообщения на эл. почту пользорвателя с инструкциями по активации")]
        public string ActivationKey { get; set; }

        [Label("Хэш-ключ")]
        [InputHidden(true)]
        [NotNull]
        [NotInput("Свойство " + nameof(Hash) + " не вводится пользователем, оно устанавливается системой при регистрации")]
        public string Hash { get; set; }

        [Label("Радио метка")]
        [InputHidden(true)]
        [NotInput("Свойство " + nameof(RFID) + " не вводится пользователем, оно устанавливается системой при регистрации служебного билета")]
        public string RFID { get; set; }




        public Account() :base(){ }
        public Account(string email, string password):base(   )
        {
            Email = email;
            Password = password;
            Hash = GetHashSha256(password);
        }

        public Account(string password)
        {
            Password = password;
        }


        /// <summary>
        /// Хэширование текстых данных
        /// </summary>
        /// <param name="text"> текст </param>
        /// <returns> результат хэширования </returns>
        public static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

    }
}
