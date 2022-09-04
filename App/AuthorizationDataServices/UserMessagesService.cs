using ApplicationDb.Entities;
 
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class UserMessagesService : IUserMessagesService
{
    private readonly AuthorizationDataModel _context;

    public UserMessagesService(AuthorizationDataModel context)
    {
        _context = context;
    }

    public int Send(Message message, int fromUserIID, int toUserId)
    {
        //message.FromUser = _context.Users.Find(fromUserIID);
        //message.ToUser = _context.Users.Find(toUserId);
        //_context.Messages.Add(message);
        return _context.SaveChanges();
    }

    /*
    public List<Message> GetInbox(int userId)
    {
        return (from p in _context.Messages.Include(m => m.ToUser) where p.ToUser.ID == userId select p).ToList();
    }

    public List<Message> GetOutbox(int userId)
    {
        return (from p in _context.Messages.Include(m => m.FromUser) where p.FromUser.ID == userId select p).ToList();
    }*/
}

