 

using System;
 
 
[EntityLabel("Бизнес показатель")]
[SearchTerms("Name")]
public class BusinessIndicator : BusinessEntity<BusinessIndicator>
{
 
    [Label("Единицы измерения")]
    [NotNullNotEmpty("Укажите единицы измерения")]
    public string Unit { get; set; }

    [NotNullNotEmpty("Наобходимо ввести описание")]
    public override string Description { get; set; }

    [Label("Отрицательные черты характера воздействия")]
    public bool IsNegative { get; set; }

}
 