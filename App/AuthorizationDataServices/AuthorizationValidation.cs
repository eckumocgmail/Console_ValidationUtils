using ApplicationDb.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class AuthorizationValidation
{
    public static void ValidateDatabaseForRoleAuthorization()
    {
        using(BusinessDataModel db = new BusinessDataModel())
        {
            AuthorizationOptions options = new AuthorizationOptions();

            BusinessResource basRole = db.BusinessResources.Where(r => r.Code == options.PublicRole).FirstOrDefault();
            if( basRole == null)
            {
                throw new Exception("Не определена базовая роль пользователя, либо отсутсвует запись о ней в базе данных");
            }
        }
    }

    public static void AddBaseRoleRecord()
    {
        using (BusinessDataModel db = new BusinessDataModel())
        {
            AuthorizationOptions options = new AuthorizationOptions();

            db.BusinessResources.Add(new BusinessResource() { 
                Name = options.PublicRole,
                Code = options.PublicRole,
                Description = "Пользователь имеют возможность обмениваться сообщениями, создавать и вступать в гуппы. "
            });
            db.SaveChanges();
        }
    }
}
