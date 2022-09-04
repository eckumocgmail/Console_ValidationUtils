using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructorApplication.Authorization
{



    /// <summary>
    /// 
    /// </summary>
    public class FackAuthFilter: IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException("Нужно реализовать фильтр авторизации ... .");
        }
    }


    /// <summary>
    /// Выполняется установка параметров проверки подлинности пользователей
    /// </summary>
    public class CustomAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException("Нужно реализовать фильтр авторизации ... .");
        }
    }



    /// <summary>
    /// Выполняет проверку в соответствии со схемой авторизации 
    /// </summary>
    public class MockAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.StatusCode = 403;                
            }
            else
            {
                switch (context.HttpContext.User.Identity.AuthenticationType)
                {
                    case "Certificate":
                        throw new NotImplementedException("Нужно реализовать фильтр авторизации ... .");
                        break;
                    case "HTTP":
                        throw new NotImplementedException("Нужно реализовать фильтр авторизации ... .");
                        break;
                    case "Cookies":
                        throw new NotImplementedException("Нужно реализовать фильтр авторизации ... .");
                        break;
                    default:
                        throw new Exception(
                            "Подозрительный тип аутентификации пользователя установлен в запросе "+
                            context.HttpContext.User.Identity.AuthenticationType);
                }
            }
        }
    }





}
