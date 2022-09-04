
using System;
using System.Collections.Generic;

 
[EntityLabel("Источник данных")]
public class BusinessDatasource: DictionaryTable 
{
          
    public BusinessDatasource()
    {
        this.Datasets = new List<BusinessDataset>();
    }

    [Label("Строка подключения")]
    [NotNullNotEmpty("Необходимо задать строку подключения")]
    public string ConnectionString { get; set; }



    [Label("Наборы данных")]
    public virtual List<BusinessDataset> Datasets { get; set; }


    /*public DatabaseManager GetOdbcDatabaseManager()
    {
        return DatabaseManager.GetOdbcDatabaseManager(ConnectionString);        
    }*/


    /*public override Dictionary<string, List<string>> ValidateOptional()
    {
        var res = new Dictionary<string, List<string>>();
        try
        {
            OdbcDataSource ds = new OdbcDataSource(ConnectionString);
            var tables = ds.GetTables();

            int ctn = tables.Count;
        }
        catch (Exception ex)
        {
            res["ConnectionString"] = new List<string>() { "Ошибка при подключении к источнику " + ex.Message };
        }
        return res;
    }*/


 
} 