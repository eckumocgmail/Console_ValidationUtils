using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class MyNavigationOptions
{      
    public string ForeignProperty { get; set; }
    public bool IsCollection { get; set; }
    public MyNavigationOptions(INavigation nav)
    {
        ForeignProperty = nav.Name+"ID";
    }
}

