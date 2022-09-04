public class ObjectValidateExtensionsTest : TestingElement
{

    public void ValidateTest() {
        new MyValidatableObject().Validate();
        Messages.Add("Реализована функция выполнения проверки свойств в объектах типа MyValidationObject");
    }
    public void EnsureIsValideTest() { }

    public override void OnTest()
    {
        EnsureIsValideTest();
        ValidateTest(); 
    }
}
