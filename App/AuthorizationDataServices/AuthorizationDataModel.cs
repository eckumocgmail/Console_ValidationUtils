using ApplicationCommon.CommonResources;

using ApplicationCore.Domain.Odbc.Controllers;
using ApplicationDb.Entities;

using AppModel.Model.BankDataModel;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Mvc_WwwLogin.Pages;

using NetCoreConstructorAngular.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


/// <summary>
/// Контекст доступа к обьектам базы данных (EntityFramework)
/// </summary>
public partial class AuthorizationDataModel : DbContext, IAuthorizationDataModel
{ 
    /// <summary>
    /// Строка соединения ADO.NET с сервером баз данных SQL Server
    /// </summary> 
    public static string DEFAULT_CONNECTION_STRING =
                                $"DataSource={"auth"}.db;Cache=Shared";
      
    public static void Configure(IConfiguration config,  IServiceCollection services)
    {
        services.AddDbContext<AuthorizationDataModel>( options => 
            options.UseSqlite($"DataSource={"auth"}.db;Cache=Shared"));
        
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddScoped(typeof(IAuthorizationDataModel),sp=>sp.GetService<AuthorizationDataModel>());
        services.AddScoped(typeof(IBusinessModel),sp=>sp.GetService<BusinessDataModel>());
        services.AddScoped<SessionManager>();
        
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<INotificationsService, NotificationsService>();
        services.AddScoped<CookieManager>();
        services.AddScoped<APIAuthorization, AuthorizationService>();
        services.AddScoped<AuthorizationService>();
        
        services.AddScoped<StatisticsService>();
        services.AddScoped<UserMessagesService>();
        services.AddScoped<UserGroupsService>();
        services.AddScoped<UserRolesService>();
        services.AddScoped<IUserMessagesService, UserMessagesService>();
        services.AddScoped<IUserGroupsService, UserGroupsService>();
        services.AddScoped<IUserRolesService, UserRolesService>();
    }


    public virtual DbSet<MessageAttribute> MessageAttributes { get; set; }
    public virtual DbSet<MessageProperty> MessageProperties { get; set; }
    public virtual DbSet<MessageProtocol> MessageProtocols { get; set; }
    public virtual DbSet<CurrencyModel> CurrencyModels { get; set; }
    public virtual DbSet<PersonWallet> PersonWallets { get; set; }
    public virtual DbSet<CurrencyUnit> CurrencyUnits { get; set; }
    //public virtual DbSet<ImageResource> ImageResources { get; set; }

    
    public virtual DbSet<ServiceMessage> News { get; set; }
    public virtual DbSet<GroupsBusinessFunctions> GroupsBusinessFunctions { get; set; }
    public virtual DbSet<BusinessLogic> BusinessLogics { get; set; }
    public virtual DbSet<BusinessDataset> BusinessDatasets { get; set; }
    public virtual DbSet<BusinessFunction> BusinessFunctions { get; set; }
    public virtual DbSet<BusinessDatasource> BusinessDatasources { get; set; }
 
    public virtual DbSet<BusinessProcess> BusinessProcesss { get; set; }
    public virtual DbSet<BusinessReport> BusinessReports { get; set; }
    public virtual DbSet<BusinessResource> BusinessResources { get; set; }
    public virtual DbSet<BusinessProcess> BusinessProcesses { get; set; }
 
    public virtual DbSet<ValidationModel> ValidationModels { get; set; }
    public virtual DbSet<BusinessData> BusinessOLAP { get; set; }
    public virtual DbSet<BusinessIndicator> BusinessIndicators { get; set; } 
    public virtual DbSet<BusinessGranularities> Granularities { get; set; }
    public virtual DbSet<BusinessData> BusinessData { get; set; }
    //public virtual DbSet<FileCatalog> FileCatalogs { get; set; }
    public virtual DbSet<FileResource> FileResources { get; set; }



    /*public virtual DbSet<ManagmentOrganization> Organizations { get; set; }
    public virtual DbSet<OrganizationDepartment> Departments { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EmployeeExpirience> EmployeeExpirience { get; set; }
    public virtual DbSet<EmployeePosition> Positions { get; set; }
    public virtual DbSet<PositionFunction> PositionFunctions { get; set; }
    public virtual DbSet<FunctionSkills> FunctionSkills { get; set; }
    public virtual DbSet<SKillExpirience> Skills { get; set; }
    public virtual DbSet<StaffsTable> Staffs { get; set; }
    public virtual DbSet<EmployeeCost> TariffRates { get; set; }
    public virtual DbSet<TimeSheet> TimeSheets { get; set; }
    public virtual DbSet<ManagmentLocation> Locations { get; set; }
    public virtual DbSet<SalaryReport> SalaryReports { get; set; }
    public virtual DbSet<ServiceMessage> News { get; set; }*/



    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<UserContext> Users { get; set; }
    public virtual DbSet<TimePoint> TimePoints { get; set; }

    
    public virtual DbSet<LoginEvents> LoginEvents { get; set; }
    public virtual DbSet<UserGroups> UserGroups { get; set; }
    public virtual DbSet<GroupMessage> GroupMessages { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<Person> Persons { get; set; }
    public virtual DbSet<Settings> Settings { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<File> Files { get; set; }

 
    public virtual DbSet<OrgWallet> OrgWallets { get; set; }

 

    public AuthorizationDataModel()
    {
        //string con = "Driver={SQL Server};" + AuthorizationDataModel.DEFAULT_CONNECTION_STRING.Replace(@"\\", @"\");
        //DatabaseManager.GetOdbcDatabaseManager(con);
    }
    public AuthorizationDataModel(DbContextOptions  options): base(options)
    {
        //string con = "Driver={SQL Server};" + AuthorizationDataModel.DEFAULT_CONNECTION_STRING.Replace(@"\\", @"\");
        //DatabaseManager.GetOdbcDatabaseManager(con);
    }

   



    public virtual List<string> GetTableNames() => this.GetEntitiesTypes().Select(t => Typing.ParseCollectionType(t)).ToList();



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Writing.ToConsole(DEFAULT_CONNECTION_STRING);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(DEFAULT_CONNECTION_STRING);
        }
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //uniq constraint
        builder.Entity<Role>()
               .HasIndex(u => u.Name)
               .IsUnique();

        //uniq constraint
        builder.Entity<Role>()
               .HasIndex(u => u.Code)
               .IsUnique();

        //uniq constraint
        builder.Entity<Account>()
               .HasIndex(u => u.Email)
               .IsUnique();


        //uniq constraint
        builder.Entity<Group>()
               .HasIndex(u => u.Name)
               .IsUnique();

        //uniq constraint
        builder.Entity<UserGroups>()
               .HasIndex(u => new { u.UserID, u.GroupID })
               .IsUnique();

        //uniq constraint
        builder.Entity<Person>()
               .HasIndex(u => new { u.FirstName, u.SurName, u.LastName, u.Birthday })
               .IsUnique();
    }



    public void TransactionCommit()
    {
        Writing.ToConsole("Сохраняем изменения ... .");
        base.SaveChanges();
    }

    public void UpdateRecord(BaseEntity baseEntity)
    {
        base.Update(baseEntity);
    }

}