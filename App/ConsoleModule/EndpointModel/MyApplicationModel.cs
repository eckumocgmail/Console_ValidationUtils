using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Коллекция сетевых сервисов
/// </summary>
[EntityLabel("Модель приложения")]
public class MyApplicationModel: Dictionary<string, MyControllerModel>
{

    public string Name { get; set; } = "Xxx";

    public MyApplicationModel() : this(Assembly.GetExecutingAssembly())
    {
       
        
    }


    public void AddAction2(MyControllerModel actionModel){
        this[actionModel.Name] = actionModel;
    }
    public MyApplicationModel(Assembly build)
    {
        this.Name = build.GetName().Name;
        build.GetControllers().Select(ctrlType=>CreateModel(ctrlType));

    }

    public static string BusinessResourceFor(Type type)
    {
        var attrs = Utils.ForType(type);
        if (attrs.ContainsKey("ForBusinessResourceAttribute"))
        {
            return attrs["ForBusinessResourceAttribute"];
        }
        else
        {
            return null;
        }
    }
    private string PathForController(Type controllerType)
    {
        string role = BusinessResourceFor(controllerType);
        if (role != null)
        {
            return "/" + role + "/" + controllerType.Name.Replace("Controller", "");
        }
        else
        {
            return "/" + controllerType.Name.Replace("Controller", "");
        }
    }
    /// <summary>
    /// <button>ok</button>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<MethodInfo> GetOwnPublicMethods(Type type)
    {
        return (from m in new List<MethodInfo>(type.GetMethods())
                where m.IsPublic &&
                        !m.IsStatic &&
                        m.DeclaringType.FullName == type.FullName
                select m).ToList<MethodInfo>();
    }
    public MyControllerModel CreateModel(Type controllerType)
    {
        var uri = "/";
        var attrs = Utils.ForType(controllerType);
        if (attrs.ContainsKey("AreaAttribute")) uri += attrs["AreaAttribute"].ToString() + "/";
        if (attrs.ContainsKey("ForBusinessResourceAttribute")) uri += attrs["ForBusinessResourceAttribute"].ToString() + "/";
        string path = PathForController(controllerType);
        Writing.ToConsole(path);
        MyControllerModel model = new MyControllerModel()
        {
            Name = controllerType.Name.Replace("`1", ""),
            Path = path,
            Actions = new Dictionary<string, MyActionModel>()
        };



        while (controllerType != null)
        {
            foreach (MethodInfo method in GetOwnPublicMethods(controllerType))
            {
                if (method.IsPublic && method.Name.StartsWith("get_") == false && method.Name.StartsWith("set_") == false)
                {

                    Dictionary<string, string> attributes = Utils.ForMethod(controllerType, method.Name);
                    Dictionary<string, object> pars = new Dictionary<string, object>();
                    model.Actions[method.Name] = new MyActionModel()
                    {
                        Name = method.Name,
                        Attributes = attributes,
                        Method = Utils.ParseHttpMethod(attributes),
                        Parameters = new Dictionary<string, MyParameterDeclarationModel>(),
                        Path = model.Path + "/" + method.Name
                    };
                    foreach (ParameterInfo par in method.GetParameters())
                    {
                        model.Actions[method.Name].Parameters[par.Name] = new MyParameterDeclarationModel()
                        {
                            Name = par.Name,
                            Type = par.ParameterType.Name,
                            IsOptional = par.IsOptional
                        };
                    }
                }
            }
            controllerType = controllerType.BaseType;
        }
        return model;
    }

    public void AddAction(MyControllerModel myControllerModel)
    {
        this[myControllerModel.Name] = myControllerModel;
    }


    public override string ToString()
    {
        string text = $"namespace {Name}\n";
        text += "{\n";
        foreach(var kv in this)
        {
            text += "\t" + kv.Value.ToString();
        }
        text += "}\n";
        return text;
    }
}

