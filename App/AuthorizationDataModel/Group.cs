 
using Newtonsoft.Json;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApplicationDb.Entities
{
    [EntityLabel("Группа пользователей")]
    public partial class Group: DictionaryTable 
    {
    

        [Label("Бизнес-процесс")]
        [SelectDataDictionary(nameof(BusinessProcess)+",Name")]
        public int? BusinessProcessID { get; set; }

        [Label("Бизнес-процесс")]
        public virtual BusinessProcess BusinessProcess { get; set; }



        [NotNullNotEmpty()]
        [UniqValidation()]
        public override string Code { get; set; }


        [NotMapped]
        [JsonIgnore()]
        public virtual List<Person> People { get; set; }

        [NotMapped]
        [JsonIgnore()]
        public virtual List<MessageProtocol> MessageProtocols { get; set; }

        [NotMapped]
        [JsonIgnore()]
        public virtual List<BusinessFunction> BusinessFunctions { get; set; }

        
        [JsonIgnore()]
        [ManyToMany("BusinessFunctions")]
        public virtual List<global::GroupsBusinessFunctions> GroupsBusinessFunctions { get; set; }

    }
}
