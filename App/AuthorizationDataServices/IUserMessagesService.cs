using System.Collections.Generic;

public interface IUserMessagesService
{
    //List<Message> GetInbox(int userId);
    // List<Message> GetOutbox(int userId);
    int Send(Message message, int fromUserIID, int toUserId);
}