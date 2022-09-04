 
using ApplicationDb.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAuthorizationDataModel: IDbContext
{
 
    public DbSet<Role> Roles { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMessage> GroupMessages { get; set; }
    public DbSet<LoginEvents> LoginEvents { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ServiceMessage> News { get; set; }    
    public DbSet<Person> Persons { get; set; }
    public DbSet<Settings> Settings { get; set; }
    public DbSet<UserContext > Users { get; set; }
    public DbSet<UserGroups> UserGroups { get; set; }        
}