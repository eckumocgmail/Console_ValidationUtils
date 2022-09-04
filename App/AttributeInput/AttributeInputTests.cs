using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
public class AttributeInputTests: TestingElement
{
   
    public class DateModel
    {
        [InputDate]
        public DateTime InputDate { get; set; }
        [InputDateTime]
        public DateTime InputDateTime { get; set; }
        [InputMonth]
        public DateTime InputMonth { get; set; }
        [InputTime]
        public DateTime InputTime { get; set; }
        [InputWeek]
        public DateTime InputWeek { get; set; }
        [InputYear]
        public DateTime InputYear { get; set; }
    }
    public class NumbersModel
    {
        [InputNumber()]
        public float InputNumber { get; set; }

        [InputPercent()]
        public int InputPercent { get; set; }
    }
    public class InputFieldsModel
    {
        [InputBool()]
        public bool InputBool { get; set; }

        [InputColor()]
        public string InputColor { get; set; }
        [InputCreditCard()]
        public string InputCreditCard { get; set; }
        [InputCurrency()]
        public string InputCurrency { get; set; }
        [InputDuration()]
        public string InputDuration { get; set; }
        [InputIcon()]
        public string InputIcon { get; set; }
        [InputImage()]
        public string InputImage { get; set; }
        [InputPostalCode()]
        public string InputPostalCode { get; set; }
    }
    public class CollectionsModel
    {
        [InputPrimitiveCollection()]
        public List<int> InputPrimitiveCollection { get; set; }

        [InputStructureCollection()]
        public List<TextModel> InputStructureCollection { get; set; }

    }

    public class TextModel
    {
        [InputEmail()]
        public string InputEmail { get; set; }

        [InputPassword()] 
        public string InputPassword { get; set; }

        [InputMultilineText()] 
        public string InputMultilineText { get; set; }

        [InputPhone()] 
        public string InputPhone { get; set; }

        [InputXml()] 
        public string InputXml { get; set; }

        [InputUrl()] 
        public string InputUrl { get; set; }

        [InputText()] 
        public string InputText { get; set; }
    }
    public override void OnTest()
    {
        Messages.Add("Реализованы функции считвания атрибутов ввода");
        typeof(TextModel).GetInputTypes().ToJsonOnScreen().WriteToConsole();
        typeof(CollectionsModel).GetInputTypes().ToJsonOnScreen().WriteToConsole();    
        typeof(InputFieldsModel).GetInputTypes().ToJsonOnScreen().WriteToConsole();
        typeof(DateModel).GetInputTypes().ToJsonOnScreen().WriteToConsole();
        typeof(InputFieldsModel).GetInputTypes().ToJsonOnScreen().WriteToConsole();

    }
}
 