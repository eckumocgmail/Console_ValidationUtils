using System;

public class ObjectCompileExpExtensionsTest : TestingElement
{

    public void CompileTest() {

        try
        {
         
            Messages.Add("УМЕЕМ ДИНАМИЧЕСКИ КОМПИЛИРОВАТЬ ВЫРАЖЕНИЯ: "+ this.Compile("GetType().Name"));

        }
        catch (Exception)
        {
            Messages.Add("Выражения не компилируются");
        }


    }

    public override void OnTest()
    {
        CompileTest();
        
    }
}
