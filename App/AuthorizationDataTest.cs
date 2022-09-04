using System;
using System.Collections.Generic;

namespace Console_Validation
{
    public class AuthorizationDataTest : TestingElement
    {
        public override void OnTest()
        {
            string fact = "Модель  аутентификапции пользотелем определяет "+
                " классы функций выполняемиыми пользователями и параметры доступа "+
                " к этим функциям";
            try
            {
                using (var model = new AuthorizationDataModel())
                {
                    model.Database.EnsureDeleted();
                    model.Database.EnsureCreated();

                    model.Trace();
                }
                Messages.Add("Истина: " + fact);
            }
            catch (Exception ex)
            {
                Messages.Add("Ложь: " + fact + ": " + ex);
                throw;

            }
        }
    }
}