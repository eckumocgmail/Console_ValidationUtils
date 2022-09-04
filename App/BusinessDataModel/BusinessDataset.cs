


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

 
[EntityLabel("Набор данных")]
[SearchTerms("Name,Description")]
public class BusinessDataset: DictionaryTable 
{
        
    [NotMapped]
    public int DatasourceID
    {
        get
        {
            return BusinessDatasourceID;
        }
        set
        {
            BusinessDatasourceID=value;
        }
    }
    [Label("Источник данных")]
    [NotNullNotEmpty("Необходимо выбрать источник")]
    [SelectDataDictionary(nameof(BusinessDatasource)+",Name")]                
    public int BusinessDatasourceID { get; set; }


    [Label("Источник данных")]    
    public virtual BusinessDatasource BusinessDatasource { get; set; }


    [Label("Скрипт")]
    [NotNullNotEmpty("Введите скрипт")]
    [InputMultilineText()]
    public string Expression { get; set; }


    [Label("Краткое описание")]
    [NotNullNotEmpty("Необходимо ввести краткое описание")]
    [InputMultilineText()]
    public override string Description { get; set; }


    [NotNullNotEmpty("Не задан целевой источник данных")]
    [NotMapped()]  
    private System.Data.DataSet DataSet;

    [NotMapped()]
    public List<string> Tables;

               
    public BusinessDataset() {
        DataSet = new System.Data.DataSet();
        Tables = new List<string>();
    }


    /// <summary>
    /// Валидация необязательных свойств
    /// </summary>
    /// <returns></returns>
    /*public override Dictionary<string, List<string>> ValidateOptional()
    {
        var res = new Dictionary<string, List<string>>();
        try
        {
            using (var db = new BusinessDataModel())
            {
                string ConnectionString = db.BusinessDatasources.Find(this.ID).ConnectionString;
                OdbcDataSource ds = new OdbcDataSource(ConnectionString);
                ds.CleverExecute(this.Expression);
            }                                    
        }
        catch (Exception ex)
        {
            res["ConnectionString"] = new List<string>() { "Ошибка при подключении к источнику " + ex.Message };
        }
        return res;
    }*/
 

  
}
 