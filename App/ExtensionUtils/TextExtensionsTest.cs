using System;

public class TextExtensionsTest : TestingElement
{

    public void GetFilePathTest() {
        try
        {
            throw new NotImplementedException();
        }
        catch(Exception ex)
        {
            this.Info(ex.ToDocument());
        }
    }
    public void GetTypesTest() { }
    public void GetTypesAllTest() { }
    public void FirstCharTest() { }
    public void CountOfCharTest() { }
    public void ReplaceAllTest() { }

    public override void OnTest()
    {
        GetFilePathTest();
        GetTypesTest();
        GetTypesAllTest();
        FirstCharTest();
        CountOfCharTest();
        ReplaceAllTest(); 
    }
}
