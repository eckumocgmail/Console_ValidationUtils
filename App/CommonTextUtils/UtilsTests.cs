 
using System;
using System.Collections.Generic;

public class UtilsTests : TestingUnit
{
    public override void OnTest()
    {

        canParseAttributes();
        canParseInputType();
        canParseDisplayNameForModel();
    }
    class ProductItemTestModel 
    { 
    }
    private void canParseAttributes()
    {
        ProductItemTestModel user = new ProductItemTestModel();
        Writing.ToConsoleJson(Utils.ForObject(user));
        Writing.ToConsoleJson(Utils.ForType(user.GetType()));
        user.GetOwnPropertyNames().ForEach((name) => {
            Writing.ToConsoleJson(Utils.ForProperty(user.GetType(), name));
        });

    }

    class TestAttributeInputClass
    {

    }
    private void canParseInputType()
    {
        var target = new TestAttributeInputClass();
        foreach (var p in target.GetType().GetProperties())
        {
            string type = Utils.GetInputType(target.GetType(), p.Name);
            if (p.Name != type &&
                new List<string> { "ID", "Description" }.Contains(p.Name) == false)
            {
                throw new Exception($"Тип элемента ввода {type} определён неверно для свойства {p.Name} класса {target.GetType().Name} ");
            }
        }

    }

    private void canParseDisplayNameForModel()
    {
        try
        {
            if (Utils.LabelFor(new TestAttributeInputClass()) != "Учетная запись")
            {
                throw new Exception("Атрибут подписи модели не был получен");
            }
            Messages.Add("Реализована возможность задавать подписи к моделям через атрибуты");
        }catch(Exception ex)
        {
            Messages.Add(ex.Message);
        }
    }
}