using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationDb.Entities
{
    [EntityLabel("Сообщение об глобальных изменени")]
    public class ServiceMessage: BaseEntity
    {


        [Label("Заголовок")]
        [NotNullNotEmpty("Необходимо указать заголовок сообщения")]
        public string Title { get; set; }


        [Label("Время")]
        [InputDateTime()]
        public DateTime Time { get; set; }


        [Label("Изображение")]
        public int? ImageID { get; set; }
        public virtual File Image { get; set; }


        [Label("URL")]
      
        [InputUrl("Значение не является URL адресом ресурса")]
        public string Href { get; set; }


        [Label("Описание")]
        [NotNullNotEmptyAttribute("Необходимо ввести описание")]
        [InputMultilineTextAttribute( )]
        public string Description { get; set; }
    }
}
