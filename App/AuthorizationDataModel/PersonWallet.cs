using ApplicationDb.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModel.Model.BankDataModel
{







    public class PersonWallet: CurrencyModel
    {            
        public int UserID { get; set; }
        public UserContext User { get; set; }

    }
}
