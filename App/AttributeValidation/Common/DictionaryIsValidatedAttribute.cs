using NetCoreConstructorAngular.Data.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalConstructor.Server.DataAttributes.ValidationAnnotations
{

    [EntityLabel("Проверка пригодности справочник")]
    [Description("При валидации выполняется проверка существования справочник," +
        "наличие доступа к нему. Дополнительно выполняется првоерка наличия данных в справочнике.")]
    public class DictionaryIsValidated : MyValidatableObject, MyValidation
    {
       
        public string Dictionary { get; set; }


        public string Property { get; set; }

        public DictionaryIsValidated()
        {

        }


        public string GetMessage(object model, string property, object value)
        {
            throw new NotImplementedException();
        }

        public string Validate(object model, string property, object value)
        {
            throw new NotImplementedException();
        }
    }
}
