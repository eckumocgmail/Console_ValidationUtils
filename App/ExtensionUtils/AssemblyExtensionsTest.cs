using System.Linq;
using System.Reflection;

public class AssemblyExtensionsTest : TestingElement
{
     
    public void GetControllersTest() {
        if (Assembly.GetExecutingAssembly().GetAttributes().Count() > 0)
            Messages.Add("Реализована функция получения атрибутов из сборки");
    }

    public override void OnTest()
    {     
        GetControllersTest(); 
    }
}
