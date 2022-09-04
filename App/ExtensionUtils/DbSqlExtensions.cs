

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DbSqlExtensions
{
    
    public static int PrepareExecute(this DbContext context, string sql)
    {
        context.Database.OpenConnection();
        var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.ExecuteScalar();
        context.Database.CloseConnection();
        return 1;
    }

    public static string GetSql(this DbContext context   )
    {
        return context.Database.GenerateCreateScript();
    }

    public static int GetVersion(this DbContext context, string sql)
    {
        return context.Database.GetAppliedMigrations().Count();
    }

    
}