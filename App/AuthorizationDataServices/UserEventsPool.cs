
public class UserEventsPool : BaseService, IEventsPool
{

    public void Recieve(int checkTimeout)
    {
        this.SetInterval(() => { this.Info("Timer"); }, checkTimeout);
    }
}