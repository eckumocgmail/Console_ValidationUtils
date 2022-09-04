using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReCaptcha
{
    public class ReCaptchaModule: IServiceModule
    {
        private ILogger<ReCaptchaModule> Logger = Factory.GetLogger<ReCaptchaModule>();
        public static void Configure(IConfiguration configuration, IServiceCollection services)
        {
            var module = new ReCaptchaModule();
            module.ConfigureServices(configuration, services);
        }
        public void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            Logger.LogInformation($"ConfigureServices()");
            services.AddSingleton(typeof(ReCaptchaOptions), sp => {
                var options = configuration.GetSection(nameof(ReCaptchaOptions)).Get<ReCaptchaOptions>();
                return options!=null? options:new ReCaptchaOptions();
            });
            services.AddScoped<ReCaptchaOptions>();
            services.AddScoped<ReCaptchaService>();
        }
    }
}
