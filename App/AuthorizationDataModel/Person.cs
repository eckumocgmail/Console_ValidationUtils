
using AppModel.Attributes.AttributeConstaints;
using Microsoft.EntityFrameworkCore;

using System;


namespace ApplicationDb.Entities
{

    [EntityLabel("Личные данные")]
   // [Index(nameof(SurName))]
    public class Person: BaseEntity
    {
        public string GetFullName()
        {
            return $"{SurName} {FirstName} {LastName}";
        }

        [InputOrder(1)]
        [Label("Фамилия")]
        [NotNullNotEmpty("Не указана фамилия пользователя")]
        [RusText("Записывайте фамилию кирилицей")]
        [Icon("person")]
        public string SurName { get; set; } = "Батов";

        [InputOrder(2)]
        [Label("Имя")]
        [NotNullNotEmpty("Не указано имя пользователя")]
        [RusText("Записывайте имя кирилицей")]
        [Icon("person")]
        public string FirstName { get; set; } = "Константин";

        [InputOrder(3)]
        [Label("Отчество")]
        [NotNullNotEmpty("Не указано отчество пользователя")]
        [RusText("Записывайте отчество кирилицей")]
        [Icon("person")]
        public string LastName { get; set; } = "Александрович";

        [InputOrder(4)]
        [Label("Дата рождения")]
        [InputDate()]
        [NotNullNotEmpty("Не указана дата рождения пользователя")]
        [Icon("person")]
        public DateTime? Birthday { get; set; } = DateTime.Parse("26.08.1989");

        [InputOrder(5)]
        [InputPhone("Номер телефона указан неверно")]
        [UniqValidation("Этот номер телефона уже зарегистрирован")]
        [Label("Номер телефона")]
        [NotNullNotEmpty("Не указана номер телефона")]
        [Icon("phone")]
        public string Tel { get; set; } = "7-904-334-1124";

        [InputOrder(6)]
        [Label("Файл")]
        [InputImage()]
        [Icon("add_a_photo")]
        public byte[] Data { get; set; } = new byte[0];

    }
}
