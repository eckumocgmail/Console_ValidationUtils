 
using System.ComponentModel.DataAnnotations.Schema;

[EntityLabel("Ресурсы предприятия")]
[SearchTerms("Name")]
[Table("Roles")]
public class Role : ActiveObject
{




    [Label("Корневой каталог")]
    [SelectDictionary("GetType().Name,Name")]
    public int? ParentID { get; set; }


    [InputHidden(true)]
    public virtual Role Parent { get; set; }

   
 
    [UniqValidation( )]
    public override string Code { get; set; } = string.Empty;


    /*
    public string GetPath(string separator)
    {
        BusinessResource parentHier =  Parent ;
        return (Parent != null) ? parentHier.GetPath(separator) + separator + Name : Name;
    }

    public override Tree ToTree()
    {
        throw new System.NotImplementedException();
    }*/
}
