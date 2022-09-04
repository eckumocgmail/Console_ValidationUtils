
[EntityIcon("home")]
[EntityLabel("Логическая переменная")]
[ClassDescription("Атрибут логическая переменной определяет способ ввода через элемент управления checkbox")]
public class InputBoolAttribute : InputTypeAttribute{
    public InputBoolAttribute() : base(InputTypes.Custom) { }     
    public override string Validate(object model, string property, object value)
    {
        return (value != null && value is bool) ? null:
            "Тип данных свойства ввода задан некорректно";
    }
    public override string GetMessage(object model, string property, object value)
    {
        return "Тип данных свойства ввода задан некорректно";
    }
}