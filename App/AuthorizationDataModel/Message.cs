using ApplicationDb.Entities;
  

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

[EntityLabel("Сообщение")]
[EntityIcon("drafts")]
[SearchTerms(nameof(Subject) + ","+ nameof(Text))]
public class Message : BaseEntity
{
    public Message(): base() {           
    }

 /*   [Label("Источник")]
    [NotNullNotEmpty("Свойство " + nameof(FromUserID) + " дожно иметь действительное значение" )]
    [NotInput("Свойство " + nameof(FromUserID) + " не вводится пользователем, оно устанавливается системой перед созданием сообщения на эл. почту пользорвателя с инструкциями по активации")]
    [NotMapped]

    public int FromUserID { get; set; }

    [Label("Источник")]        
    [NotInput("Свойство " + nameof(FromUser) + " не вводится пользователем, оно устанавливается системой перед созданием сообщения на эл. почту пользорвателя с инструкциями по активации")]
    [JsonIgnore()]
    [NotMapped]
    public virtual UserContext  FromUser { get; set; }

    [Label("Назначение")]
    [NotMapped]
    [SelectDictionary(nameof(UserContext ) + ",GetFullName()")]
    public int ToUserID { get; set; }

    [NotMapped]
    [JsonIgnore()]
    public virtual UserContext  ToUser { get; set; }*/


    [Label("Создано")]
    [InputHidden()]
    public DateTime Created { get; set; } = DateTime.Now;


    [Label("Тема")]
    [NotNullNotEmpty("Необходимо указать тему сообщения")]
    public string Subject { get; set; }


    [Label("Текст сообщения")]
    [InputMultilineText( )]
    [NotNullNotEmpty("Необходимо ввести текст сообщения")]
    public string Text { get; set; }

 
}
 
