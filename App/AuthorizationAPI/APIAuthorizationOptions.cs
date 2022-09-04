using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public interface APIAuthorizationOptions
    {
        int CheckTimeout { get; }
        bool LogginAuth { get; }
        int SessionTimeout { get; }
    }
