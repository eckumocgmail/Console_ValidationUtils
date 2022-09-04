using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CommonTextUtilsTests : TestingUnit
{
    public CommonTextUtilsTests()
    {
        Push(new BinaryEncoderTest());
        Push(new CharacterEncoderTest());
        Push(new NamingTest());
        Push(new WritingTest());
        Push(new RusEngTransliteTest());
    }
}