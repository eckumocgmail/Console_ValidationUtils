using ConsoleApp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConsoleProgramTest : TestingUnit
{
    public ConsoleProgramTest()
    {
        canRunAsConsole();
    }

    private void canRunAsConsole()
    {
        ConsoleProgram.RunInteractive<AssemblyNavigator>();
    }
}