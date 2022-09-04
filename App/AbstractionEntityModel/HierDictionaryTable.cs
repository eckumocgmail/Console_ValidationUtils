

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

/// <summary>
/// Иерархическое преждставление набора данных
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHier<out TItem> 
{
    public static Type GetProductType() => typeof(TItem);
}
public class HierDictionaryTable<T> : DictionaryTable, IHier<T>
{

    [Label("Корневой каталог")]
    [InputHidden(true)]
    [NotInput("")]
    public int? ParentID { get; set; }

    [InputHidden(true)]
    [NotInput("")]
    [JsonIgnore()]
    public virtual T Parent { get; set; }

    public virtual string GetPath(string separator)
    {
        HierDictionaryTable<T> parentHier = ((HierDictionaryTable<T>)((object)Parent));
        return (Parent != null) ? parentHier.GetPath(separator) + separator + Name : Name;
    }

    public BaseEntity GetRoot()
    {
        BaseEntity p = this;
        while (p.GetValue("ParentID") != null && p.GetValue("ParentID") != p.GetValue("ID"))
        {
            p.Join("Parent");
            p = (BaseEntity)p.GetValue("Parent");
        }
        return p;
    }

    public List<HierDictionaryTable<T>> GetChildren()
    {
        /*var children = new List<HierDictionaryTable<T>>();
        using (var db = new ApplicationDbContext())
        {
            children = ((IQueryable<HierDictionaryTable<T>>)(db.GetDbSet(typeof(T).Name))).Where(p => p.ParentID == ID).ToList();
        }
        return children;*/
        throw new Exception("GetChildren()");
    }


    /*public Tree ToTree()
    {
        var ChildNodes = new List<ViewNode>();
        GetChildren().ForEach(p => ChildNodes.Add(p.ToTree()));
        return new Tree()
        {
            Item = this,
            Children = ChildNodes
        };
    }*/

    

}