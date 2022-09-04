using System;

internal class ViewItem
{
    public bool HasRegistered { get; internal set; }
    public UserModelsService Client { get; internal set; }
    public System.Func<object, object> GetSession { get; internal set; }
    public int Code { get; internal set; }
    public bool ViewInitiallized { get; internal set; }

    internal void RemoveFromParent()
    {
        throw new NotImplementedException();
    }

    internal bool WasChanged()
    {
        throw new NotImplementedException();
    }
}