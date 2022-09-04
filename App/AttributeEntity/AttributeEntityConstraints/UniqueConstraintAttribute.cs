using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 

/// <summary>
/// Ограничения уникальности свойств
/// </summary>
public class UniqueConstraintAttribute: ModelCreatingAttribute
{
    private readonly string[] _columns;

    public UniqueConstraintAttribute( string sequence )
    {
        _columns = sequence.Split(",");            
             
    }
}
 
