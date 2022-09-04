using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

/// <summary>
/// Модель параметров вызова удаленной процедуры
/// </summary>
public class MyActionModel
{
    public MyActionModel()
    {
    }

    public MyActionModel(MethodInfo method)
    {
        this.Name = method.Name;
        this.Path = $"/{method.DeclaringType.Name.Replace("Controller", "")}/{method.Name}";
        this.Attributes = Utils.ForMethod(method.DeclaringType,method.Name);
        foreach(var par in method.GetParameters())
        {
            var p = new MyParameterDeclarationModel(par);
            Parameters[par.Name] = p;
        }
    }

    [Icon("home")]
    [Label("Наименование")]
    [EngText()]
    [InputText()]
    [NotNullNotEmpty("Необходимо ввести имя")]
    public virtual string Name { get; set; } = "Index";

    [Icon("account_tree")]
    [Label("Путь")]
    [InputText()]
    [NotNullNotEmpty("Необходимо ввести путь")]
    public virtual string Path { get; set; } = "/Index";

    public virtual string Method { get; set; } = "Get";

    [NotMapped]
    [JsonIgnore]
    public virtual List<string> PathStr
    {
        set
        {
            string spath = "";
            value.ForEach(p => { spath += "/" + p; });
            Path = spath;
        }
    }



    public string CsInterface
    {
        get
        {
            string cmd = "";
            foreach(var kv in Parameters)
            {
                cmd += $",{kv.Value.Type} {kv.Key}";
            }
            cmd = cmd.Length > 0 ? cmd.Substring(1) : cmd;
            return $"\t\tpublic IActionResult {Name}({cmd})"+"\n\t\t{\n\t\t}\n\n";
        }
    }


    public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, MyParameterDeclarationModel> Parameters { get; set; } = new Dictionary<string, MyParameterDeclarationModel>();

    public override string ToString()
    {
        return CsInterface;
    }


}
