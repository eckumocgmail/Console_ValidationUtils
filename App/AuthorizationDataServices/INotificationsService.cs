public interface INotificationsService
{
    void InfoMessage(string v);
    void ErrorMessage(string v);
    void WarnMessage(string v);
    void SubmitDialog(string v);
    void InputDialog(string v); 
}

public class NotificationsService: INotificationsService
{
    public NotificationsService( APIAuthorization authorization )
    {
        
    }

    public void InfoMessage(string v)
    {
        throw new System.NotImplementedException();
    }

    public void ErrorMessage(string v)
    {
        throw new System.NotImplementedException();
    }

    public void WarnMessage(string v)
    {
        throw new System.NotImplementedException();
    }

    public void SubmitDialog(string v)
    {
        throw new System.NotImplementedException();
    }

    public void InputDialog(string v)
    {
        throw new System.NotImplementedException();
    }
}