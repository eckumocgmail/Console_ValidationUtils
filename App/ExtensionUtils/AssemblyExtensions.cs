using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Расширения сборок
/// </summary>
public static class AssemblyExtensions
{
    public static IEnumerable<string> GetNames( this Assembly assembly ) => assembly.GetTypes().Select(t => t.GetTypeName());
    public static IEnumerable<Type> GetClassTypes(this Assembly assembly) =>
        assembly.GetTypes().Where(t => t.IsClass == true && ("" + t.Name[0]).IsEng());
    /// <summary>
    /// Вывод документации по сборе
    /// </summary>    
    public static void Trace( this Assembly target )
    {
        target.GetTypes()
            .Where(type => type.GetTypeName().IsEng())
            .Select(type => type.ToDocument())
            .ToList()
            .ForEach(text => Console.WriteLine(text));
    }

    public static HashSet<Type> Get<ServiceType>(this Assembly target) => target.GetTypes<ServiceType>();
    public static HashSet<Type> GetTagHelpers(this Assembly target) => target.GetTypes<TagHelper>();
    public static HashSet<Type> GetEventArgs(this Assembly assembly) => assembly.GetTypes().Where(t => Typing.IsExtendedFrom(t, typeof(EventArgs))).ToHashSet();
    public static HashSet<Type> GetAttributes(this Assembly assembly) => assembly.GetTypes().Where(t => Typing.IsExtendedFrom(t, typeof(Attribute))).ToHashSet();
    public static HashSet<Type> GetInputAttributes(this Assembly assembly) => assembly.GetTypes().Where(t => Typing.IsExtendedFrom(t, typeof(InputTypeAttribute))).ToHashSet();
    public static HashSet<Type> GetControlAttributes(this Assembly assembly)
        => assembly.GetTypes().Where(t => Typing.IsExtendedFrom(t, typeof(ControlAttribute))).ToHashSet();


    /// <summary>
    /// Метод получения контроллеров объявленных в сборке, находящейся в файле по заданному адресу
    /// </summary>
    /// <param name="filename"> адрес файла сборки </param>
    /// <returns> множество контроллеров </returns>
    public static HashSet<Type> GetDataContexts(this Assembly assembly)
        => assembly.GetTypes().Where(t => Typing.IsExtendedFrom(t, "DbContext")).ToHashSet();



    /// <summary>
    /// Метод получения контроллеров объявленных в сборке, находящейся в файле по заданному адресу
    /// </summary>
    /// <param name="filename"> адрес файла сборки </param>
    /// <returns> множество контроллеров </returns>
    public static HashSet<Type> GetViewComponents(this Assembly assembly)
        => assembly.GetTypes().Where(t => Typing.IsExtendedFrom(t, typeof(ViewComponent))).ToHashSet();


    /// <summary>
    /// Метод получения контроллеров объявленных в сборке, находящейся в файле по заданному адресу
    /// </summary>
    /// <param name="filename"> адрес файла сборки </param>
    /// <returns> множество контроллеров </returns>
    public static HashSet<Type> GetHubs(this Assembly assembly)
        => assembly
            .GetTypes()
            .Where(t => Typing.IsExtendedFrom(t, typeof(Hub)))
            .ToHashSet();


    /// <summary>
    /// Метод получения контроллеров объявленных в сборке, находящейся в файле по заданному адресу
    /// </summary>
    /// <param name="filename"> адрес файла сборки </param>
    /// <returns> множество контроллеров </returns>
    public static HashSet<Type> GetControllers(this Assembly assembly)
        => assembly
            .GetTypes()
            .Where(t => Typing.IsExtendedFrom(t, "ControllerBase"))
        .ToHashSet();


    /// <summary>
    /// Метод получения контроллеров объявленных в сборке, находящейся в файле по заданному адресу
    /// </summary>
    /// <param name="filename"> адрес файла сборки </param>
    /// <returns> множество контроллеров </returns>
    public static HashSet<Type> GetPages(this Assembly assembly)
        => assembly
            .GetTypes()
            .Where(t => Typing.IsExtendedFrom(t, typeof(PageModel)))
        .ToHashSet();

    /// <summary>
    /// Метод получения контроллеров объявленных в сборке, находящейся в файле по заданному адресу
    /// </summary>
    /// <param name="filename"> адрес файла сборки </param>
    /// <returns> множество контроллеров </returns>
    public static HashSet<Type> GetTypes<BaseType>(this Assembly assembly)
        => assembly
            .GetTypes()
            .Where(t => Typing.IsExtendedFrom(t, typeof(BaseType)))
            .ToHashSet();

    /// <summary>
    /// Метод получения контроллеров объявленных в сборке, находящейся в файле по заданному адресу
    /// </summary>
    /// <param name="filename"> адрес файла сборки </param>
    /// <returns> множество контроллеров </returns>
    public static HashSet<Type> GetValidationAttributes(this Assembly assembly)
        => assembly.GetTypes().Where(t => Typing.IsExtendedFrom(t, nameof(ValidationAttribute))).ToHashSet();

    /// <summary>
    /// Подключение сборки из файла
    /// </summary>
    public static Assembly LoadAssembly(this string path) => Assembly.LoadFile(path);

    /// <summary>
    /// Blazor компоненты привязанные к маршруту
    /// </summary>
    public static IEnumerable<Type> GetPages(this Assembly assembly, string PagesNameSpace)
        => assembly.GetTypes().Where(t => t.Namespace == PagesNameSpace && t.IsAutoClass == false && t.IsClass == true && t.Name.Contains("+") == false && t.IsClass == true && t.Name.Contains("<") == false).ToList();

}