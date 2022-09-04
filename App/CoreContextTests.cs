using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CoreContextTests : TestingUnit
{
    public CoreContextTests()
    {
        Push(new AsyncContextTest());
    }
}