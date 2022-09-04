 

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
 

[EntityLabel("Отчёт")]
[SearchTerms("Name,Description")]
public class BusinessReport: BusinessEntity<BusinessReport>
{
    [Label("Источники данных")]
    public virtual List<BusinessDatasource> BusinessDatasources { get; set; }

    [Label("Наборы данных")]
    [NotMapped()]
    public virtual List<BusinessDataset> BusinessDatasets { get; set; }


    [NotNullNotEmpty("Необходимо описать функиональность")]
    [InputMultilineText()]
    [Label("Описание отчёта")]
    public override string Description { get; set; }

    [NotNullNotEmpty("Необходимо загрузить XML-документ")]
    [InputMultilineText()]     
    [Label("XML схема отчёта")]        
    public string Xml { get; set; }

   
}
 
