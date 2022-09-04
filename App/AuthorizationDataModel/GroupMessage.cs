using ApplicationDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDb.Entities
{
    [EntityLabel("Сообщения в группе")]
    public class GroupMessage: Message
    {
        public int GroupID { get; set; }
        public virtual Group Group { get; set; }
    }
}
