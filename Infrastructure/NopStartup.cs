
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.CodeInjector.Services;

namespace Nop.Plugin.Misc.CodeInjector.Infrastructure
{

    public class NopStartup : INopStartup
    {
        public int Order => 100;

        public void Configure(IApplicationBuilder application)
        {
            
        }

        
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICodeInjectorService, CodeInjectorService>();
            services.AddScoped< CIParseService>();
        }
    }
}