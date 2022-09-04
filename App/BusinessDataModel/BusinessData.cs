using System;
using System.Collections.Generic;
using System.Text;

//Загрузка данных начинаетя с этой таблицы"
[EntityLabel("Входящая информация")]
public class BusinessData : BaseEntity
{

    [NotNullNotEmpty("Показатель является обязательным полем")]
    [SelectDataDictionary(nameof(BusinessIndicator) + ",Name")]
    public int BusinessIndicatorID { get; set; }
    public virtual BusinessIndicator BusinessIndicator { get; set; }


    [NotNullNotEmpty("Набор данных является обязательным полем")]
    [SelectDataDictionary(nameof(BusinessDataset) + ",Name")]
    public int BusinessDatasetID { get; set; }
    public virtual BusinessDataset BusinessDataset { get; set; }


    [NotNullNotEmpty("Периодичность является обязательным полем")]
    [SelectDictionary(nameof(BusinessGranularities) + ",Name")]
    public int GranularityID { get; set; }


    [NotNullNotEmpty("Обьект мониторинга является обязательным полем")]
    [SelectDictionary(nameof(BusinessResource) + ",Name")]
    public int BusinessResourceID { get; set; }
    public virtual BusinessResource BusinessResource { get; set; }

    [NotNullNotEmpty("Начало периода")]
    [InputDate()]    
  
    public DateTime BeginDate { get; set; }




    [InputNumber()]
    [NotNullNotEmpty()]
    public float IndValue { get; set; }



    [NotInput()]
    public DateTime Changed { get; set; } = DateTime.Now;
}

