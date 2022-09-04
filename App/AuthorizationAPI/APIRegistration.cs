using ApplicationDb.Entities;
using System;
using System.Collections.Generic;
using System.Text;


public interface APIRegistration
{
    string Signup(Account account );
    string Signup(Account account,Person person);
    void Signup(string Email, string Password, string Confirmation,
                string SurName, string FirstName, string LastName, DateTime Birthday, string Tel);
    bool HasUserWithEmail(string email);
    bool HasUserWithActivationKey(string activationKey);
    bool HasUserWithTel(string tel);
    UserContext  GetUserByEmail(string email);
    UserContext  GetUserByTel(string tel);
    string GetHashSha256(string password);
    string GenerateRandomPassword(int length);
    string GenerateActivationKey(int length);
    void RestorePasswordByEmail(string email);
}
