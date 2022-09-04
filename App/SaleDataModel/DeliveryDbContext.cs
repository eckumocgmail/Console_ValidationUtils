using static System.Console;
 
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using DataCommon;
using System.ComponentModel.DataAnnotations.Schema;
using Mvc_Apteka.Entities;
using DataEntities;

public class DeliveryDbContext : DeliveryDbContext<ProductCustomer<SaleItem>, SaleItem, ProductHolder<SaleItem>>
{
    public DeliveryDbContext()
    {
    }

    public DeliveryDbContext(Action<DbContextOptionsBuilder> configure) : base(configure)
    {
    }

    public DeliveryDbContext(DbContextOptions options) : base(options)
    {
    }
}

namespace DataEntities
{

    /// <summary>
    /// 
    /// </summary>
    public interface IDeliveryDbContext<TCustomer, TItem, THolder>: IDbContext
            where TCustomer : ProductCustomer<TItem>
            where TItem : SaleItem
            where THolder : ProductHolder<TItem>
    {
        public DbSet<TItem> OrderItems { get; set; }

        public DbSet<SaleOrder<TItem>> Orders { get; set; }
        public DbSet<THolder> Holders { get; set; }
        public DbSet<ProductTransport> Transports { get; set; }
        public DbSet<ProductItem> Products { get; set; }
        public DbSet<ProductsInStock> ProductsInStock { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<TCustomer> Customers { get; set; }
    }



    /// <summary>
    /// 
    /// </summary>
    public class DeliveryDbContext<TCustomer, TItem, THolder> : BaseDbContext,
        IDeliveryDbContext<TCustomer, TItem, THolder>
            where TCustomer : ProductCustomer<TItem>
            where TItem : SaleItem
            where THolder : ProductHolder<TItem>
    {
        
        public virtual DbSet<ProductActivity> Activities { get; set; }
        public virtual DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public virtual DbSet<TItem> OrderItems { get; set; }
        public virtual DbSet<SaleOrder<TItem>> Orders { get; set; }        
        public virtual DbSet<THolder> Holders { get; set; }
        public virtual DbSet<ProductTransport> Transports { get; set; }
        public virtual DbSet<ProductInfo> ProductInfos { get; set; }
        public virtual DbSet<ProductItem> Products { get; set; }
        public virtual DbSet<ProductsInStock> ProductsInStock { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<TCustomer> Customers { get; set; }

        [NotMapped]
        private Action<DbContextOptionsBuilder> configure;

        public DeliveryDbContext(): this((options) => {
            //options.UseInMemoryDatabase(nameof(DeliveryDbContext<TCustomer, TItem, THolder>));
            options.UseSqlite("DataSource=delivery.db;Cache=Shared");
        }){}

        public DeliveryDbContext( Action<DbContextOptionsBuilder> configure ) : base()
        {
            this.configure = configure;
        }
        public DeliveryDbContext(DbContextOptions options) : base(options)
        {
        }
        public dynamic GetDbSet(string name)
        {
            if (typeof(TCustomer).GetTypeName().Equals(name)) return this.Customers;
            if (typeof(THolder).GetTypeName().Equals(name)) return this.Holders;
            switch (name)
            {
                case nameof(ProductItem): return this.Products;
                case nameof(ProductImage): return this.ProductImages;
                case nameof(ProductTransport): return this.Transports;
                case nameof(SaleOrder<TItem>): return this.OrderItems;
                case nameof(DataEntities.ProductsInStock): return this.ProductsInStock;
                default: throw new KeyNotFoundException(name);
            }
            throw new KeyNotFoundException(name);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (optionsBuilder.IsConfigured == false)
            {
                configure(optionsBuilder);
            }
        }       
    }



    /*public IDictionary<string, int> InitPrimaryData(IDeliveryDbContextInitiallizer<TCustomer, TItem, THolder> initiallizer, IWebHostEnvironment env)
    {
        string contentPath = env.ContentRootPath.ReplaceAll(@"\\", @"/").ReplaceAll(@"\", @"/");
        WriteLine(contentPath);

        return InitPrimaryData(initiallizer, contentPath);
    }

    public IDictionary<string, int> InitPrimaryData(IDeliveryDbContextInitiallizer<TCustomer, TItem, THolder> initiallizer, string contentPath)
    {

        return initiallizer.Init(this, contentPath);
    }

    private T Resolve<T>()
    {
        throw new Exception();
        //return this.provider.GetService<T>();
    }

    */





    /*public static void ConfigureDeliveryServices(IServiceCollection services, IConfiguration config)
    {
        // services.AddDbContext<DeliveryDbContext<ProductCustomer<OrderItem>, OrderItem, Holder<OrderItem>>>(options => options.UseInMemoryDatabase("h"), ServiceLifetime.Scoped);
        WriteLine(nameof(TCustomer), nameof(TItem), nameof(THolder));

        services.AddDbContext<DeliveryDbContext<TCustomer, TItem, THolder>>(options => options.UseInMemoryDatabase("DbContext"), ServiceLifetime.Scoped);

        //services.AddScoped<IDeliveryDbContextInitiallizer<ProductCustomer<OrderItem>, OrderItem, Holder<OrderItem>>,
        //  DeliveryDbContextInitiallizer<ProductCustomer<OrderItem>, OrderItem, Holder<OrderItem>>>();
        //services.AddScoped<
        //  DeliveryDbContextInitiallizer<ProductCustomer<OrderItem>, OrderItem, Holder<OrderItem>>>();




        services.AddScoped<IKeywordsParserService, StupidKeywordsParserService>();

        services.AddScoped(
            typeof(IEntityFasade<ProductCustomer<TItem>>),
            sp => new UnitOfWork<Order<TItem>>(
                sp.GetService<DeliveryDbContext<TCustomer, TItem, THolder>>()));
        services.AddScoped(
            typeof(IEntityFasade<Holder<TItem>>),
            sp => new UnitOfWork<Holder<TItem>>(
                sp.GetService<DeliveryDbContext<TCustomer, TItem, THolder>>()));
        services.AddScoped(typeof(IEntityFasade<Product>), sp => new UnitOfWork<Product>(sp.GetService<DeliveryDbContext<TCustomer, TItem, THolder>>()));
        services.AddScoped(typeof(IEntityFasade<ProductImage>), sp => new UnitOfWork<ProductImage>(sp.GetService<DeliveryDbContext<TCustomer, TItem, THolder>>()));
        services.AddScoped(typeof(IEntityFasade<ProductsInStock>), sp => new UnitOfWork<ProductsInStock>(sp.GetService<DeliveryDbContext<TCustomer, TItem, THolder>>()));
        services.AddScoped(typeof(IEntityFasade<Order<TItem>>), sp => new UnitOfWork<Order<TItem>>(sp.GetService<DeliveryDbContext<TCustomer, TItem, THolder>>()));
        services.AddScoped(typeof(IEntityFasade<TItem>), sp => new UnitOfWork<TItem>(sp.GetService<DeliveryDbContext<TCustomer, TItem, THolder>>()));
        services.AddScoped(typeof(IEntityFasade<ProductCustomer<TItem>>), sp => new UnitOfWork<ProductCustomer<TItem>>(sp.GetService<DeliveryDbContext<TCustomer, TItem, THolder>>()));
        //services.AddScoped<UnitOfWork>();
        services.AddSingleton<IEventsService, EventsService>();
        services.AddScoped<IDeliveryDbContextInitiallizer<TCustomer, TItem, THolder>, DeliveryDbContextInitiallizer<TCustomer, TItem, THolder>>();
        services.AddScoped<IKeywordsParserService, StupidKeywordsParserService>();


        //OrderConsumer<TItem>.AddOrderConsumer(services, "http://localhost:8080");
    }*/
}
