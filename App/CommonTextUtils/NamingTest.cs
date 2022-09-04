 
public class NamingTest : TestingUnit
{
    public override  void OnTest()
    {            
    }


    protected  void canConvertIdentifier()
    {
        if (Naming.ToCamelStyle("HomeController") != "homeController")
            Messages.Add("Не удалось применить CamelStyle");
        Messages.Add("Есть функция получения идентификатора в форме CamelStyle");
        if (Naming.ToCamelStyle("HomeController") != "homeController")
            Messages.Add("Не удалось применить CamelStyle");
        Messages.Add("Есть функция получения идентификатора в форме CamelStyle");
    }

    private void canConvertNameToDiffrentStyles()
    {
        string capitalStyle = "AppModule";
        string snakeStyle = "app_module";
            
        string kebabStyle = "app-module";
        string camelStyle = "appModule";

        string lastname = camelStyle;
        if (snakeStyle != (lastname = Naming.ToSnakeStyle(lastname)))
            throw new System.Exception($"{lastname}!={snakeStyle}");           
        if (kebabStyle != (lastname = Naming.ToKebabStyle(lastname)))
            throw new System.Exception();
        if (camelStyle != (lastname = Naming.ToCamelStyle(lastname)))
            throw new System.Exception();
        if (capitalStyle != (lastname = Naming.ToCapitalStyle(lastname)))
            throw new System.Exception();

        this.Messages.Add($"can convert name: {lastname} to diffrent styles");
    }

}

 
