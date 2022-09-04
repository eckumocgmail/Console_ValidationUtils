 
using ApplicationDb.Entities;
 
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;




public class RegistrationService : APIRegistration
{
    private IAuthorizationDataModel _db;
    private readonly EmailService _email;
    private readonly AuthorizationOptions _options;

    public RegistrationService(IAuthorizationDataModel db, AuthorizationOptions options, EmailService email)
    {
        _db = db;
        _email = email;
        _options = options;
    }


    /// <summary>
    /// Восстановление пароля по электронной почте
    /// </summary>
    /// <param name="email"></param>
    public void RestorePasswordByEmail(string email)
    {
        UserContext user = this.GetUserByEmail(email);
        if (user == null)
        {
            throw new Exception("Пользователь с таким адресом электронной почты не зарегистрирован");                
        }
        string password = GenerateRandomPassword(10);
        _db.Accounts.Find(user.Account.ID).Hash = GetHashSha256(password);
        _db.SaveChanges();
        _email.SendEmail(email, "Восстановление пароля", "Пароль от Вашей учетной записи: " + password);
    }

    /// <summary>
    /// Проверка регистрации пользователя с заданным электронным адресом
    /// </summary>
    /// <param name="Email">электронный адрес</param>
    /// <returns></returns>
    public bool HasUserWithEmail(string Email)
    {
        return (from user
                            in _db.Users.Include(a => a.Account)
                where user.Account.Email == Email
                select user).Count() > 0;

    }


    /// <summary>
    /// Проверка наличия пользователя с заданным номером телефона
    /// </summary>
    /// <param name="tel">номер телефона в формате x-xxx-xxx-xxxx </param>
    /// <returns></returns>
    public bool HasUserWithTel(string tel)
    {
        return (from user
                            in _db.Users.Include(a => a.Account)
                where user.Person.Tel == tel
                select user).Count() > 0;

    }


    /// <summary>
    /// Получение данных пользователя по адресу электронной почты,
    /// данный метод регистрозависимый, т.е для поиска нужно указать адрес электронной почты 
    /// в том же регистре в котором он был зарегистрирован.
    /// </summary>
    /// <param name="email">адрес электронной почты</param>
    /// <returns></returns>
    public UserContext GetUserByEmail(string email)
    {

        ApplicationDb.Entities.Account account = (from p in _db.Accounts where p.Email == email select p).FirstOrDefault();
        if (account == null)
        {
            return null;
        }
        else
        {
            return (from p in _db.Users
                                .Include(a => a.Account)
                                .Include(a => a.Settings)
                                .Include(a => a.Person)
                                .Include(a => a.Role)
                                .Include(a => a.UserGroups)
                    where p.AccountID == account.ID select p).FirstOrDefault();
        }

    }


    /// <summary>
    /// Получение данных пользователя по номеру телефона, номер телефона регистрируется в формате 7-XXX-XXX-XXXX
    /// </summary>
    /// <param name="tel">номер телефона</param>
    /// <returns></returns>
    public UserContext GetUserByTel(string tel)
    {
        Person person = (from p in _db.Persons where p.Tel == tel select p).FirstOrDefault();
        if (person == null)
        {
            return null;
        }
        else
        {
            return (from p in _db.Users
                                .Include(a => a.Account)
                                .Include(a => a.Settings)
                                .Include(a => a.Person)
                                .Include(a => a.Role)
                                .Include(a => a.UserGroups)
                    where p.PersonID == person.ID
                    select p).FirstOrDefault();
        }
    }


    /// <summary>
    /// Метод применения функции хеширования символов
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public string GetHashSha256(string password)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(password);
        SHA256Managed hashstring = new SHA256Managed();
        byte[] hash = hashstring.ComputeHash(bytes);
        string hashString = string.Empty;
        foreach (byte x in hash)
        {
            hashString += String.Format("{0:x2}", x);
        }
        return hashString;
    }


    /// <summary>
    /// Метод генерации случайного пароля заданной длины
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public string GenerateRandomPassword(int length)
    {
        Random random = new Random();
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower() +
                        "0123456789";
        return new string(Enumerable.Repeat(chars, length)
                            .Select(s => s[random.Next(s.Length)]).ToArray());

    }


    /// <summary>
    /// Проверка наличия пользователя с зарегистриваронным ключом активации
    /// </summary>
    /// <param name="activationKey">ключ актвации</param>
    /// <returns>true, если такой ключ уже зарегистрирован</returns>
    public bool HasUserWithActivationKey(string activationKey)
    {
        ApplicationDb.Entities.Account account = (from p in _db.Accounts where p.ActivationKey == activationKey select p).FirstOrDefault();
        return account != null;
    }


    /// <summary>
    /// Генерация уникального ключа активации учетной записи
    /// </summary>
    /// <param name="length">длина ключа</param>
    /// <returns></returns>
    public string GenerateActivationKey(int length)
    {
        string key = null;
        do
        {
            key = RandomString(length);
        } while (this.HasUserWithActivationKey(key));
        return key;
    }

    /// <summary>
    /// Получение случайной последовательности символов
    /// </summary>
    /// <returns> последовательность символов </returns>
    private string RandomString(int keylength)
    {
        Random random = new Random();
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower() +
                        "0123456789";
        return new string(Enumerable.Repeat(chars, keylength)
                            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public Role GetDefaultRole()
    {
        return (from p in _db.Roles where p.Code == _options.PublicRole select p).FirstOrDefault();
    }

    public void Signup(ApplicationDb.Entities.Account account, Person person)
    {
        Signup(account, person, GetDefaultRole());
    }

    public void Signup(ApplicationDb.Entities.Account account, Person person, Role role)
    {
 
        Settings settings = new Settings();
     
        if( role == null)
        {
            _db.Roles.Add(new Role() { 
                Name = _options.PublicRole,
                Code = _options.PublicRole,
                Description = ""
            });
            _db.SaveChanges();
            role = (from r in _db.Roles where r.Code == _options.PublicRole select r).FirstOrDefault();
        }

        
        UserContext user = new UserContext()
        {
            Person = person,            
            Account = account,
            Settings = settings,
            Role = role,
            LastActive = GetTimestamp(),
            LoginCount = 0,
            IsActive = false
        };

        Group group = (from g in _db.Groups where g.Name == _options.PublicGroup select g).FirstOrDefault();


        _db.Persons.Add(person);
        _db.Accounts.Add(account);
        _db.Settings.Add(settings);
        _db.Users.Add(user);
        _db.SaveChanges();
    }

    public void Signup(string Email, string Password, string Confirmation, 
        string SurName, string FirstName, string LastName, DateTime Birthday, string Tel)
    {

        ApplicationDb.Entities.Account account = new ApplicationDb.Entities.Account()
        {
            Email = Email,
            Hash = GetHashSha256(Password)
        };
        Person person = new Person()
        {
            SurName = SurName,
            FirstName = FirstName,
            LastName = LastName,
            Birthday = Birthday,
            Tel = Tel
        };
        Signup(account, person);
    }

    /// <summary>
    /// Получечние текущего времени в милисекундах
    /// </summary>
    /// <returns></returns>
    private long GetTimestamp()
    {
        return (long)(((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0))).TotalMilliseconds);
    }

    public MethodResult<string> Signup(Account account)
    {
        throw new NotImplementedException();
    }

    MethodResult<string> APIRegistration.Signup(Account account, Person person)
    {
        throw new NotImplementedException();
    }
}
