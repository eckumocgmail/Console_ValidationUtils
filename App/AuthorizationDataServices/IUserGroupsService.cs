using ApplicationDb.Entities;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public interface IUserGroupsService
{
    List<BusinessFunction> GetBusinessFunctions(int userId);
    Group GetGroup(int id);
    List<GroupMessage> GetGroupMessages(int groupId, int page, int size);
    List<Group> GetGroups();
    List<MessageProtocol> GetMessageProtocolsForUser(int userId);
    JArray GetMessagesForBusinessFunction(BusinessFunction function);
    List<Person> GetPersons(int groupId);
    List<Group> GetUserGroups(int userId);
    string GetUsername(int userId);
    bool IsUserInGroup(int groupId, int userId);
    void JoinToGroup(int groupId, int userId);
    void LeaveGroup(int groupId, int userId);
    void PublishIntoGroup(int userId, int groupId, Message message);
}