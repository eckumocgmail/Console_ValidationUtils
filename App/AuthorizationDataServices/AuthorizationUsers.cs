using ApplicationDb.Entities;


using Microsoft.Extensions.Logging;


/// <summary>
/// Служба уровня приложения, содержит пользователей сеансов
/// </summary>
public class AuthorizationUsers: AuthorizationCollection<UserContext>, APIUsers
{
    public AuthorizationUsers( ILogger<AuthorizationUsers> logger, AuthorizationOptions options ): base( null, options){}

    public string FindByEmail(string email)
    {
        throw new System.NotImplementedException();
    }

    public string FindByEmail(string host, string email)
    {
        throw new System.NotImplementedException();
    }
}

