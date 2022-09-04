public class ObjectInputExtensionsTest : TestingElement
{

    public void GetUserInputPropertyNamesTest() {
        typeof(MyValidatableObject).GetUserInputPropertyNames().ToJsonOnScreen().WriteToConsole();
        Messages.Add("Реализована функция считывания имен свойств подлежащих вводу");
    }
    public void IsCollectionTypeTest() {
        Messages.Add("Реализована функция определения свойств типа перечислений");
    }
    public void GetAttrsTest() {
        Messages.Add("Реализована функция считывания получения значений атрибутов");
    }
 
 

    public override void OnTest()
    {
        GetUserInputPropertyNamesTest();
        IsCollectionTypeTest();
        GetAttrsTest(); 

    }
}
