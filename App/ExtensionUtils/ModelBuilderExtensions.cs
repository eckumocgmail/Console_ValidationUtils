 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
public static class ModelBuilderExtensions
{

    /// <summary>
    /// Учёт постоянных затрат на оплату труда
    /// </summary>   
    public static void UseFile(this DbContextOptionsBuilder builder, string filepath)
    {       
        if (System.IO.File.Exists(filepath) == false)
        {
            using (Stream fs = System.IO.File.Create(filepath))
            {
                fs.Flush();
            }
        }
        builder.UseSqlite($"DataSource={filepath};Cache=Shared");
    }

    /// <summary>
    /// Связывание контекста данных с базой SQL-Lite, представленной в виде файлового источника.
    /// Если файла не существует, то будет создан новый
    /// </summary>
    public static void AddFileDatabase<DbContextType>(this IServiceCollection services, string filepath = null)
        where DbContextType : DbContext
    {
        if (filepath == null)
            filepath = typeof(DbContextType).GetTypeName().ToKebabStyle() + ".db";
        services.AddDbContext<DbContextType>(options => options.UseFile(filepath));
    }

    /// <summary>
    /// Применение атрибутов
    /// </summary>
    public static void ApplyAnnotations( this ModelBuilder builder, DbContext context )
    {   
        foreach (var type in context.GetEntitiesTypes())
        {
            Dictionary<string, string> dictionary = GetEntityContrainsts(type);
            foreach (var p in dictionary)
            {
                switch (p.Key)
                {                    
                    case nameof(UniqueConstraintAttribute):
                        builder.Entity(type).HasIndex(p.Value.Split(",")).IsUnique();
                        break;
                    case nameof(SearchTermsAttribute):
                        builder.Entity(type).HasIndex(p.Value.Split(",")).IsUnique();
                        break;
                    default:
                        break;
                }
            }
        }       
    }


    /// <summary>
    /// Считывание атрибутов
    /// </summary>
    public static Dictionary<string, string> GetEntityContrainsts(Type type)
    {
        Dictionary<string, string> attrs = new Dictionary<string, string>();
        foreach (var data in type.GetCustomAttributesData())
        {
            if (data.AttributeType.IsExtendsFrom(typeof(ConstraintAttribute).Name))
            {
                foreach (var arg in data.ConstructorArguments)
                {
                    string value = arg.Value.ToString();
                    attrs[data.AttributeType.Name] = value;
                }
            }

        }
        return attrs;
    }
}
 