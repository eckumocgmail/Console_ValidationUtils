using System;

public class ThrowableExtensionsTest : TestingElement
{

    public void ToHTMLTest()
    {
        try
        {
            throw new Exception();

        }catch(Exception ex)
        {
            ex.ToHTML().WriteToConsole();
            Messages.Add("Реализована функция формирования HTML для исключений");
        }

    }
 

    public override void OnTest()
    {
        ToHTMLTest();
        
    }
}
