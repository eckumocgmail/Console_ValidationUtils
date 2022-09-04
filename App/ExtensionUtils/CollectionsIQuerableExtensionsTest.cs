using System;
using System.Collections.Generic;
using System.Linq;

public class CollectionsIQuerableExtensionsTest : TestingElement
{

    public void GetPageTest() {
        try
        {
            var list = new List<int>();
            for (int i = 0; i < 100; i++)
                list.Add(i);
            if (list.GetPage<int>(1, 10).Count() != 10)
            {
                throw new System.Exception("Метод постраничного просмотра коллекции не работает");
            }
            Messages.Add("Реализован метод постраничного просмотра сущностей для любых коллекций");
        }
        catch (Exception ex)
        {
            Messages.Add(ex.Message);
        }
    }


    public override void OnTest()
    {
        GetPageTest();
        
    }
}
