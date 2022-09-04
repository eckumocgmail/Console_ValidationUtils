
using System;
using System.Collections.Generic;

public class BusinessDataTest : TestingElement
{
    public override void OnTest()
    {
        string fact = "Бизнес-модель описывает процесс информационного "
            +" взаимодействия с помощью блоков( определяющих операцию, " +
            "бизнес ресурс выполняющий обработку инфомрации, управляющий" +
            "поток информации, поток входящей информации, поток исходящей информации, " +
            "поток исключительной информации)";
        try
        {
            using (var model = new BusinessDataModel())
            {
                model.Database.EnsureDeleted();
                model.Database.EnsureCreated();

                model.MessageAttributes.ToJsonOnScreen().WriteToConsole();
            }
            Messages.Add("Истина: " + fact );
        }
        catch(Exception ex)
        {
            Messages.Add("Ложь: "+fact+": "+ex);
            throw;
        }
    }
}
