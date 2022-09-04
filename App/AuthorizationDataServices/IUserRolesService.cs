using ApplicationDb.Entities;
using System.Collections.Generic;

public interface IUserRolesService
{
    Role CreateBusinessResource(string name, string description, string code);
    Role FindBusinessResourceByCode(string code);
    Role GetBusinessResourceByCode(string roleCode);
    List<string> GetUserBusinessResourceCodes(UserContext user);
    //List<ViewNode> GetUserBusinessResourceNavigation(string BusinessResourceName);
}