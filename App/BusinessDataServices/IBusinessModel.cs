 

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

public interface IBusinessModel: IDbContext
{
    public DbSet<GroupsBusinessFunctions> GroupsBusinessFunctions { get; set; }

    public DbSet<BusinessDatasource> BusinessDatasources { get; set; }
    public DbSet<BusinessFunction> BusinessFunctions { get; set; }
    
    public DbSet<BusinessLogic> BusinessLogics { get; set; }

    public DbSet<BusinessProcess> BusinessProcesss { get; set; }
    public DbSet<BusinessReport> BusinessReports { get; set; }
    public DbSet<BusinessResource> BusinessResources { get; set; }
    public DbSet<BusinessProcess> BusinessProcesses { get; set; }


    public DbSet<MessageAttribute> MessageAttributes { get; set; }
    public DbSet<MessageProperty> MessageProperties { get; set; }
    public DbSet<MessageProtocol> MessageProtocols { get; set; }
    public DbSet<ValidationModel> ValidationModels { get; set; }
    public DbSet<BusinessData> BusinessData { get; set; }
    public DbSet<BusinessIndicator> BusinessIndicators { get; set; }
    public DbSet<BusinessDataset> BusinessDatasets { get; set; }
 
    public DbSet<BusinessGranularities> Granularities { get; set; }

 
}
