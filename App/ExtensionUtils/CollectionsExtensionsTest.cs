using System;
using System.Collections.Generic;
using System.Linq;

public class CollectionsExtensionsTest : TestingElement
{

    public void ForEachTest() {
        IEnumerable<int> numbers = new HashSet<int>() { 1, 2, 3, 4 };
        numbers.ForEach(Console.WriteLine);
        Messages.Add("Реализована фуцнкция передора перечисляемых объектов");
    }

    public void PrintTest() {
        IEnumerable<int> numbers = new HashSet<int>() { 1, 2, 3, 4 };
        numbers.Print();
        Messages.Add("Реализована фуцнкция печати перечисляемых объектов");

    }

    public void AddRangeTest() {
        HashSet<int> numbers = new HashSet<int>() { 1, 2, 3, 4 };
        numbers.AddRange<int>("7,8,9".Split(",").Select(t=>int.Parse(t)));
        Messages.Add("Реализована фуцнкция добавления множеств");
    }

    public override void OnTest()
    {
        ForEachTest();
        PrintTest();
        ForEachTest();
        AddRangeTest();
    }
}
