using ApplicationUnit.Encoder;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BinaryEncoderTest: TestingElement
{
    public override void OnTest()
    {
        var encoder = new BinaryEncoder();
        encoder.ToBinary("This is a test").WriteToConsole();
        Messages.Add("Реализованы функции бинарного ввода вывода");
        

    }
}