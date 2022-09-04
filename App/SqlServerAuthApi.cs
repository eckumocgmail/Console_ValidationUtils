using DataADO;


using DataCommon.DatabaseMetadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataModels
{
    internal class SqlServerAuthApi : SqlServerWebApi
    {

        public static bool Created = false;
         
        internal SqlServerAuthApi()
        {
            Init();
        }

        internal void CreateDatabaseFile(string database)
        {
            TryPrepareQuery(@$"CREATE DATABASE {database}");
        }

        internal SqlServerAuthApi(string server, string database) : base(server, database)
        {
            Init();
        }

        internal SqlServerAuthApi(string server, string database, bool trustedConnection, string userID, string password) : base(server, database, trustedConnection, userID, password)
        {
            Init();
        }

        private void Init()
        {
            if(Created==false)
            {
                DropTables();
                //AddEntityType(typeof(Account));
                //AddEntityType(typeof(Login));
                //AddEntityType(typeof(Person));
                //AddEntityType(typeof(User));
                UpdateDatabase();
                CreateServices();
                Created = true;
            }
             
        }

        internal void DropTables()
        {
            GetTablesMetadata().ToList().ForEach(md => {
                PrepareQuery($"DROP TABLE [{md.Value.TableSchema}].[{md.Value.TableName}]");
            });
           
        }

        private void CreateServices()
        {
            foreach(var EntityType in EntityTypes)
            {
                var metadata = DDLFactory.CreateTableMetaData(EntityType);
                var fasade = new EntityFasade(this, new TableMetadata(metadata), EntityType);
                this.Services.Add(fasade);
            }
        }

        private TableMetadata FindMetaData(Type entityType)
        {
            GetTablesMetadata().ToJsonOnScreen().WriteToConsole();
            return GetTablesMetadata().Where(meta => meta.Value.TableName == entityType.Name).Select(kv=>kv.Value).FirstOrDefault();
        }
    }
}
