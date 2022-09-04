using ApplicationDb.Entities;
using System.Collections.Generic;


/// <summary>
/// Параметры жизненого цикла обьектов сеанса
/// </summary>
public class AuthorizationOptions
{
    public bool LogginAuth { get; set; }
    public long SessionTimeout { get; set; }
    public int KeyLength { get; set; }
    public string UserCookie { get; set; }
    public string ServiceCookie { get; set; }
    public int CheckTimeout { get; set; }
    public string ApplicationUrl { get; set; }
    public List<string> OAuthProviders { get; set; } = new List<string>();

    /// <summary>
    /// Роль пользователя по умолчанию,
    /// присваивается пользователям после 
    /// проведеня процедуры регистрации
    /// </summary>
    public string PublicRole { get; set; }
    public string PublicGroup { get; set; }
        

    public AuthorizationOptions()
    {
        this.LogginAuth = false;
        this.SessionTimeout = 10000;
        this.KeyLength = 32;
        this.UserCookie = "UserKey";
        this.ServiceCookie = "ServiceKey";
        this.CheckTimeout = 1000;
        this.PublicRole = "User";
        this.PublicGroup = "User";            
    }

    /// <summary>
    /// Страница авторизации
    /// </summary>
    public string LoginPagePath { get; set; } = "/Account/Login";

    /// <summary>
    /// Страница активации учетной записи
    /// </summary>
    public string ActivationRequirePath { get; set; } = "/Account/ActivationRequire";
    public string AccessDeniedPath { get; internal set; } = "/Account/AccessDenied";


    /// <summary>
    /// Маршруты только для авторизованных пользователей
    /// </summary> 
    public Dictionary<string, List<string>> RoleValidationRoutes
        = new Dictionary<string, List<string>>() {
            { "Dev", new List<string>{ "/DevFace" } },
            { "DBA", new List<string>{ "/DBAFace" } },
            { "SI", new List<string>{ "/SIFace" } },
            { "SA", new List<string>{ "/SAFace" } },
            { "User", new List<string>{ "/UserFace" } },
            { "Admin", new List<string>{ "/AdminFace" } }
              
    };
}
