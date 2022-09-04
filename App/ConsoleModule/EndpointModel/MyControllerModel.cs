
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Модель mvc-контроллера /[controller]/[action]
/// </summary>
public class MyControllerModel: MyActionModel
{
    [Icon("home")]
    [Label("Наименование")]
    [EngText()]
    [InputText()]
    [NotNullNotEmpty("Необходимо ввести имя")]
    public override string Name { get; set; }
    
    [InputIcon()]
    [Label("Иконка на панель инструментов")]
    public string Icon { get; set; }


    [Icon("home")]
    [Label("Наименование")]
    [EngText()]
    [InputMultilineText()]
    [NotNullNotEmpty("Необходимо ввести имя")]
    public string Description { get; set; }


    [Icon("account_tree")]
    [Label("Путь")]
    [InputText()]
    [NotNullNotEmpty("Необходимо ввести путь")]
    public override string Path { get; set; }



    [NotNullNotEmpty("Необходимо зарегистрировать операции")]
    [Label("Поддерживаемые операции")]
    [InputStructureCollection()]
    public Dictionary<string, MyActionModel> Actions { get; set; }
            = new Dictionary<string, MyActionModel>();


    [InputStructureCollection()]
    [NotNullNotEmpty("Необходимо зарегистрировать операции")]
    [Label("Поддерживаемые операции")]    
   
    public IList<string> Services { get; set; } = new List<string>();


    public MyControllerModel() { }
    public MyControllerModel(Type type)
    {
        type.GetOwnMethodNames().ForEach(m =>
        {
            var method = type.GetMethods().FirstOrDefault(met => met.Name == m);
            this.Actions[method.Name] = new MyActionModel(method);
        });
    }

    public IEnumerable<MyActionModel> GetMyActions()
    {
        var actionList = new List<MyActionModel>();
        foreach(var p in Actions)
        {
            actionList.Add(p.Value);
        }
        return actionList.ToArray();
    }


    /// <summary>
    /// Запись информации о методах в справочник
    /// </summary>
    /// <param name="data"></param>
    public void WriteTo(IDictionary data)
    {
        Actions.ToList().ForEach(a =>
        {
            data[a.Key] = Formating.ToJson(a.Value);
        });
    }



    public MyApplicationModel ToApplication()
    {
        var app = new MyApplicationModel();
        Assembly.GetExecutingAssembly().GetControllers().Select(ctrl=>new MyControllerModel(ctrl));
       
        return app;
    }


    public override string ToString()
    {

        string text = $"\n\tpublic class {Name}\n";
        text += "\t{\n";

        foreach(var action in Actions)
        {
            text += "\n" + action.Value.ToString();
        }

        text += "\t}\n";
        return text;
    }

    /*public string GetAnnotationForService()
    {
        return "@Injectable({ providedIn: 'root' })\n";
    }


    public string GetImportsForService()
    {
        return
            "import { Observable } from 'rxjs';\n" +
            "import { Injectable } from '@angular/core';\n" +
            "import { HttpClient } from '@angular/common/http';\n\n";
    }*/
}