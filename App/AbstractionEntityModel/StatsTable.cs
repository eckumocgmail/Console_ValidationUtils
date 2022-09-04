using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
/// <summary>
/// Сущность реализующая данный клас хранит 
/// колличественные характеристики выполнения 
/// бизнес функций обьектами информационного взаимодействия
/// за оределённый промежуток времени, заданный 
/// свойствами [Begin, EndDate).
/// 
/// Важно! 
/// Обратите внимание что конечный точка EndDate исключена из промежутка.
/// </summary>
public class StatsTable
{
    [Label("Начало периода")]
    [DataType(DataType.Date)]
    public DateTime BeginDate { get; set; }

    [Label("Начало периода")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }


    public List<string> GetDimensions()
    {
        List<string> dims = new List<string>();
        foreach (var nav in GetNavigation( ))
        {
            var propertyType = this.GetType().GetProperty(nav.Name).GetType();
            if (Typing.HasBaseType(propertyType, typeof(DictionaryTable)))
            {
                dims.Add(nav.Name);
            }
        }
        return dims;
    }

    public IEnumerable<INavigation> GetNavigation()
    {
        Type type = GetType();
        var result = new List<INavigation>();
        WithDbContext((db) => {
            result.AddRange(db.Model.GetEntityTypes().Where(t => t.Name == type.Name).FirstOrDefault().GetNavigations());
        });
        return result;
    }

    public void WithDbContext(Action<DbContext> todo )
    {
        using (DbContext db = GetDbContext())
        {
            todo(db);
        }
    }

    private DbContext GetDbContext()
    {
        return (DbContext)GetType().Assembly.GetDataContexts().Where(db => db.GetEntityTypeNames().Contains(GetType().Name)).FirstOrDefault().New();
    }

    public List<string> GetIndicators()
    {
        return (from p in GetType().GetProperties() where Typing.IsNumber(p) select p.Name).ToList();
    }

}
