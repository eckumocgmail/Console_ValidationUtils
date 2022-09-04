public class TypeAttributesExtensionTest : TestingElement
{

 

    public override void OnTest()
    {
        Utils.ForType(typeof(MyValidatableObject));
        Messages.Add("Реализована функция получения атрибутов для типов"); 
    }
}
