 
using ApplicationDb.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
 
public interface APIAuthorization:  APIRegistration
{
    bool IsConfirmedAccountRequire { get; }

    public bool IsSignin();
    public bool InBusinessResource(string roleName);
    public bool IsActivated();
    public UserContext Signin(string RFIDLabel);
    public UserContext Signin(string Email, string Password, bool? IsFront=false);
    public void Signout(bool? IsFront = false);
    public UserContext Verify(bool? IsFront = false);
    public ConcurrentDictionary<string, object> Session();
    public Task<Account> GetAccountByID(int iD);
    Task<string> GenerateEmailConfirmationTokenAsync(Account user);
    
    Task<IEnumerable<string>> GetExternalAuthenticationSchemesAsync();
}
