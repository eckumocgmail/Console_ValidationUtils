using ApplicationDb.Entities; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


public class StatisticsService
{
    private readonly ILogger<StatisticsService> _logger;

    public StatisticsService(ILogger<StatisticsService> logger) {
        _logger = logger;
        
    }

 

    public async Task SaveStatisticsOLAP()
    {
        await Task.CompletedTask;
        using (var _context = new AuthorizationDataModel())
        {
            /*foreach (Type entityType in _context.GetFactsTables())
            {
                var fact = entityType;
                var dims = ReflectionService.CreateWithDefaultConstructor<BaseEntity>(entityType).GetDimensions();                
            }

            await _context.SaveChangesAsync();*/
        }
    }

 
     


       

         
}
