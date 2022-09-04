public class TextNamingExtensionsTest : TestingElement
{

    public void ParseStyleTest() { 
        if("ToString".ParseStyle().ToString()!= "Capital")
        {
            Messages.Add("Реализована функция определения стиля записи идентификатора");
        }
        else
        {
            Messages.Add("Не реализована функция определения стиля записи идентификатора");

        }
    }
 
    public override void OnTest()
    {
        ParseStyleTest();
        
    }
}
