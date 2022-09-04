using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
public class EntityNavigationAttribute: Attribute
{
    private readonly string _propertyName;

    public EntityNavigationAttribute(string propertyName )
    {
        _propertyName = propertyName;
    }
}
 
