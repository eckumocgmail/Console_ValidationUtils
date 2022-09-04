using ApplicationDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


[EntityLabel("Много ко многим")]
public class ServiceGroups : BaseEntity
{
    public int ServiceID { get; set; }

    [JsonIgnore()]
    public virtual Service Service { get; set; }
    public int GroupID { get; set; }

    [JsonIgnore()]
    public virtual Group Group { get; set; }
}