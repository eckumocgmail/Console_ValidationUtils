using ApplicationDb.Entities;




public interface APIUsers : APIActiveCollection<UserContext >
{


    string FindByEmail(string email);
    string FindByEmail(string host,string email);


}
