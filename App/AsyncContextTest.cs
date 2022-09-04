using System.Collections.Generic;

public class AsyncContextTest : TestingElement
{
    public override void OnTest()
    {
        var context = new AsyncContext();
        string key = context.Put((message) => { 
        });
        key.WriteToConsole();
        Messages.Add("Реализована функция выдачи ключей доступа к асинхронным операциям");
        

    }
}