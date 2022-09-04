public class TextTypeExtensionsTest : TestingElement
{

    public void IsTypeTest() { }
    public void IsLinearOperationTest() {

        if ("+".IsLinearOperation())
        {
            Messages.Add("Реализована функция определения арифметических опереторов");
        }
        else
        {
            Messages.Add("Не реализована функция определения арифметических опереторов");
        }
    }
    public void IsNumberTest() {
        if ("-110".IsNumber())
        {
            Messages.Add("Реализована функция определения чисел в тексте");
        }
        else
        {
            Messages.Add("Не реализована функция определения чисел в тексте");
        }
    }
    public void IsDateTest()
    {
        if ("26.08.1989".IsNumber())
        {
            Messages.Add("Реализована функция определения дат в тексте");
        }
        else
        {
            Messages.Add("Не реализована функция определения дат в тексте");

        }
    }
 

    public override void OnTest()
    {
        IsTypeTest();
        IsLinearOperationTest();
        IsLinearOperationTest();
        IsNumberTest();
        IsNumberTest();
        IsDateTest();
        
    }
}
