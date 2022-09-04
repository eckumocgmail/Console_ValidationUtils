
using System;
using System.Collections.Concurrent;

public class MyValidatableAttribute : Attribute, IDisposable
{
    private static string Trace = "";
    public static ConcurrentStack<MyValidatableAttribute> Objects =
        new ConcurrentStack<MyValidatableAttribute>();

    private static int Counter = 0;

    private int Index = -1;

    public MyValidatableAttribute()
    {
        this.Index = Counter++;
        Objects.Push(this);
        Trace += this.ToJsonOnScreen();
        this.Info(Trace);
    }

    public void Dispose()
    {
        MyValidatableAttribute removed = null;
        Objects.TryPeek(out removed);
        if (removed != null)
        {
            if (removed.GetHashCode() != this.GetHashCode())
            {
                this.Info("");
            }
        }
    }

    public override string ToString()
    {
        return $"[{Index}].[{GetType().GetTypeName()}].[{GetHashCode()}]";
    }

}
