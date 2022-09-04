

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BusinessDataModel : AuthorizationDataModel, IBusinessModel, IDbContext
{
    public override DbSet<GroupsBusinessFunctions> GroupsBusinessFunctions { get; set; }
    public override DbSet<BusinessDatasource> BusinessDatasources { get; set; }
    public override DbSet<BusinessFunction> BusinessFunctions { get; set; }
    public override DbSet<BusinessLogic> BusinessLogics { get; set; }
    public override DbSet<BusinessProcess> BusinessProcesss { get; set; }
    public override DbSet<BusinessReport> BusinessReports { get; set; }
    public override DbSet<BusinessResource> BusinessResources { get; set; }
    public override DbSet<BusinessProcess> BusinessProcesses { get; set; }
    public override DbSet<MessageAttribute> MessageAttributes { get; set; }
    public override DbSet<MessageProperty> MessageProperties { get; set; }
    public override DbSet<MessageProtocol> MessageProtocols { get; set; }
    public override DbSet<ValidationModel> ValidationModels { get; set; }
    public override DbSet<BusinessData> BusinessOLAP { get; set; }
    public override DbSet<BusinessIndicator> BusinessIndicators { get; set; }
    public override DbSet<BusinessDataset> BusinessDatasets { get; set; }
    public override DbSet<BusinessGranularities> Granularities { get; set; }
    public override DbSet<BusinessData> BusinessData { get; set; }


    public BusinessDataModel() { }
    public BusinessDataModel(DbContextOptions options) : base(options)
    {        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured == false)
            optionsBuilder.UseSqlite(
                "DataSource=business.db;Cache=Shared"
            );
    }

          
    public override List<string> GetTableNames()
        => this.GetEntitiesTypes().Select(t => Typing.ParseCollectionType(t)).ToList();
    public void Update(BaseEntity baseEntity) => base.Update(baseEntity);


}