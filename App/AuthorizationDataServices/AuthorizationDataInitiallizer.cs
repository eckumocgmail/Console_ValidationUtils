using ApplicationDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModel 
{
    
}
public class AuthorizationDataInitiallizer
{
    /// <summary>
    /// 
    /// </summary>
    public static void InitPrimaryData()
    {
        using (var db = new AuthorizationDataModel())
        {
            db.Database.EnsureCreated();

            InitBaseAccount(db);
            InitBusinessResources(db);
        }
    }


    /// <summary>
    /// Инициаллизация пользовательских прав доступа к функциям приложения
    /// </summary>
    private static void InitBusinessResources()
    {

        using (var db = new AuthorizationDataModel())
        {
            InitBusinessResources(db);

        }
    }

    private static void InitBusinessResources(AuthorizationDataModel db)
    {
        Writing.ToConsole("Инициаллизация пользовательских прав доступа к функциям приложения");
        if (db.Roles.Count() < 3)
        {
            Role users;
            Role admins;
            Role analitics;
            db.Roles.Add(users = new Role()
            {
                Name = "Личный кабинет",
                Code = "User",
                Description = "Базовый полномочия, которые распостраняются на всех сотрудников"
            });
            db.Roles.Add(analitics = new Role()
            {
                Name = "Аналитические материалы",
                Code = "Analitic",
                Description = "Бизнес аналитик, исследует системные процессы",
                Parent = users
            });
            db.Roles.Add(admins = new Role()
            {
                Name = "Администрирование функций",
                Code = "Admin",
                Description = "Управление отчётными формами, управления ресурсами организации подразделениями, должностями, штатным расписанием.",
                Parent = analitics
            });
            db.Roles.Add(new Role()
            {
                Name = "Разработка",
                Code = "Developer",
                Description = "Разработка функциональной модели предприятия.",
                Parent = admins
            });


            db.SaveChanges();

        }
    }
    /// <summary>
    /// "Регистрация тестовой учетной записи)"
    /// </summary>
    private static void InitBaseAccount()
    {

        using (var db = new AuthorizationDataModel())
        {
            InitBaseAccount(db);

        }
    }

    private static void InitBaseAccount(AuthorizationDataModel db)
    {
        InitBusinessResources(db);

        if (db.Accounts.Where(a => a.Email.ToLower() == "eckumocuk@gmail.com").Any() == false)
        {

            Writing.ToConsole("\n\nРегистрация тестовой учетной записи...");
            var role = (from r in db.Roles where r.Code == "Developer" select r).SingleOrDefault();
            var account = new Account("eckumocuk@gmail.com", "eckumocuk@gmail.com");
            account.Activated = DateTime.Now;
            account.ActivationKey = "this is a test";
            var person = new Person()
            {
                FirstName = "Константин",
                LastName = "Александрович",
                SurName = "Батов",
                Birthday = new DateTime(1970, 1, 1),
                Tel = "7-777-777-7777"
            };

            var settings = new Settings();
            UserContext user = new UserContext()
            {
                Person = person,
                Account = account,
                Settings = settings,
                Role = role,
                LastActive = DateTimeOffset.Now.ToUnixTimeMilliseconds(),

                LoginCount = 0,
                IsActive = false
            };

            db.Persons.Add(person);
            db.Accounts.Add(account);
            db.Settings.Add(settings);
            db.Users.Add(user);
            db.SaveChanges();

            db.Groups.Add(new Group()
            {
                Name = "Разработчик",
                Description = "hi"
            });
            db.SaveChanges();

            db.UserGroups.Add(new UserGroups()
            {
                GroupID = db.Groups.First().ID,
                UserID = db.Users.First().ID
            });
            db.SaveChanges();

        }
    }

}