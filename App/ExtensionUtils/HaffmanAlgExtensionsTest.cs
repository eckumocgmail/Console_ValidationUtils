using ApplicationUnit.Encoder;

using System;

public class HaffmanAlgExtensionsTest : TestingElement
{

    public void SeriallizeTest() {
        try
        {
            var encoder = new CharacterEncoder();
            string code = encoder.Encode("ABBCCCDDDDEEEEE");
            encoder.Decode(code).WriteToConsole();
            Messages.Add("Реализована функция сжатия текста");

        }
        catch (Exception)
        {
            Messages.Add("Не удалось сжать текст");
        }
    }
 
 


    public override void OnTest()
    {
        SeriallizeTest(); 
    }
}
