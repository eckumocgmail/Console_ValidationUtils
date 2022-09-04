using System;
using System.Collections.Generic;
using System.Text;


[EntityLabel("Паралельность(периодичность)")]
[SystemEntity()]
public class BusinessGranularities : DictionaryTable 
{


    [UniqValidation()]
    public override string Code { get; set; }

}