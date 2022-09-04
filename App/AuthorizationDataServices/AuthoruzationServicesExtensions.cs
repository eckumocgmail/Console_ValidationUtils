using ApplicationCore.Domain.Odbc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.DataSource
{
    public static class AuthoruzationDataModelExtensions
    {


        public static void GetDatabaseManager(this IServiceCollection services)
        {
            services.AddScoped<IUserMessagesService,    UserMessagesService>();
            services.AddScoped<IUserGroupsService,      UserGroupsService>();
            services.AddScoped<IUserRolesService,       UserRolesService>();       
        }
 
    }
}
