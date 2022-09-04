using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuthorization.IBusinessModel
{
    public class BusinessActivity
    {
        public BusinessResource Resource { get; set; }
        public BusinessFunction Function { get; set; }
        public MessageProtocol Protocol { get; set; }
    }
}
