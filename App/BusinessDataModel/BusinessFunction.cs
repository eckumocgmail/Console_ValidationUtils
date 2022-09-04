 


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

[EntityLabel("Бизнес функция")]
[SearchTerms("Name")]
public class BusinessFunction : BusinessEntity<BusinessFunction>
{

    /// <summary>
    /// Возвращает список всех дочерних функций
    /// </summary>
    /// <returns></returns>
    public List<BusinessFunction> FlatList()
    {
        List<BusinessFunction> functions = new List<BusinessFunction>();
        this.SubFunctions.ForEach(sf => {
            functions.AddRange(sf.FlatList());
        });
        functions.Add(this);
        return functions;
    }



    [NotMapped( )]
    public List<BusinessFunction> SubFunctions { get; set; } = new List<BusinessFunction>();

    [ManyToMany("BusinessFunctions")]
    public virtual List<global::GroupsBusinessFunctions> GroupsBusinessFunctions { get; set; }


    public virtual BusinessResource BusinessResource { get; set; }
    public virtual BusinessLogic BusinessLogic { get; set; }


    [InverseProperty("To")]
    public virtual MessageProtocol Input { get; set; }

    [InverseProperty("From")]
    public virtual MessageProtocol Output { get; set; }



    /// <summary>
    /// Возвращает протокол отчётных форм, определённый как входящая информация для данной бизнес функции
    /// </summary>
    /// <returns></returns>
    /*public Form ForInput()
    {
        using (var db = new BusinessDataModel())
        {
            var prot = (from p in db.MessageProtocols where p.ToBusinessFunctionID == this.ID select p).SingleOrDefault();
            return null;
        }
    }*/
    public MessageProtocol GetInputProtocol()
    {
        using (var db = new BusinessDataModel())
        {
            return (from p in db.MessageProtocols where p.ToBusinessFunctionID == this.ID select p).SingleOrDefault();
        }
    }

    /// <summary>
    /// Возвращает протокол отчётных форм, определённый как исходящая информация для данной бизнес функции
    /// </summary>
    /// <returns></returns>
    public MessageProtocol ForOutput()
    {
        using (var db = new BusinessDataModel())
        {
            return (from p in db.MessageProtocols where p.FromBusinessFunctionID == this.ID select p).SingleOrDefault();
        }
    }

    public override string GetPath(string separator)
    {        
        BusinessFunction parentHier = Parent;
        return (Parent != null) ? parentHier.GetPath(separator) + separator + Name : Name;
    }


    

    public List<BusinessFunction> GetSubfunctions()
    {
        if(SubFunctions == null)
        {
            using(var db = new BusinessDataModel())
            {
                SubFunctions = (from p in db.BusinessFunctions where p.ParentID == ID select p).ToList();
            }
            
        }
        return SubFunctions;
    }
}
 