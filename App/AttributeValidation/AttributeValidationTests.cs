using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AttributeValidationTests: TestingUnit
{
    public override void OnTest()
    {
        Messages.Add("Реализованы функции проверки данных по атрибутам");
        canIdentifyRusWords();
        canIdentifyEngWords();
        canIdentifyRusText();
        canIdentifyEngText();
    }

    public class Model
    {
        [RusWord]
        public string RusWord { get; set; }

        [EngWord]
        public string EngWord { get; set; }

        [RusText]
        public string RusText { get; set; }

        [EngText]
        public string EngText { get; set; }
    }
    private void canIdentifyEngWords()
    {
    }

    private void canIdentifyRusWords()
    {
    }

    private void canIdentifyEngText()
    {
    }

    private void canIdentifyRusText()
    {
    }
}