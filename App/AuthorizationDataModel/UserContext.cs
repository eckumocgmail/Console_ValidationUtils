



using ApplicationDb.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace ApplicationDb.Entities
{


    

}
/// <summary>
/// Обьект модели пользователя сеансов
/// </summary>
[EntityLabel("Пользователь")]
[EntityIcon("build")]
public class UserContext : ActiveObject
{

    public string GetFullName()
    {
        Join("Person");
        return Person.GetFullName();
    }

    public UserContext()
    {
        UserGroups = new List<UserGroups>();
        //Inbox = new List<Message>();
        //Outbox = new List<Message>();
        Name = "[user]";
    }

    public UserContext(Role role, Person person, Account account, Settings settings)
    {
        UserGroups = new List<UserGroups>();
        Role = role;
        Person = person;
        Account = account;
        Settings = settings;
        //Inbox = new List<Message>();
       //Outbox = new List<Message>();
    }




    [Label("Учетная запись")]
    public int AccountID { get; set; }

    [InputHidden(true)]
    [Label("Учетная запись")]
    public virtual Account Account { get; set; }


    [Label("Роль")]
    public int RoleID { get; set; }

    [InputHidden(true)]
    [Label("Роль")]
    public virtual Role Role { get; set; }


    [Label("Настройки")]
    public int SettingsID { get; set; }
    [Label("Настройки")]
    public virtual Settings Settings { get; set; }


    [Label("Личная инф.")]
    public int PersonID { get; set; }

    [Label("Личная инф.")]
    public virtual Person Person { get; set; }


    [NotMapped]
    [Label("Группы")]
    public virtual List<Group> Groups { get; set; } = new List<Group>();


    [Label("Группы")]
    [NotMapped]
    public int UserGroupsID { get; set; }

    [Label("Группы")]
    [ManyToMany("Groups")]
    [InputHidden(true)]
    public virtual List<UserGroups> UserGroups { get; set; } = new List<UserGroups>();


    [Label("Кол-во посещений")]
    public int LoginCount { get; set; }








    [NotMapped]
    [Label("Выполняемые функции")]
    public virtual List<BusinessFunction> BusinessFunctions { get; set; } = new List<BusinessFunction>();



    public string GetHomeUrl()
    {
        return $"/{this.Role.Code}Face/{this.Role.Code}/{this.Role.Code}Home";
    }
}