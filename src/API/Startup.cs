using API.Configuration.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace API
{
    public class Startup
    {
        private static ILogger _logger;
        private static ILogger _loggerForApi;
        
        public Startup(IConfiguration configuration)
        {
            ConfigureLogger();
            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerDocumentation();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private static void ConfigureLogger()
        {
            _logger =  new LoggerConfiguration()   
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.RollingFile(new CompactJsonFormatter(),"logs/logs")
                .CreateLogger();

            _loggerForApi = _logger.ForContext("Module", "API");

            _loggerForApi.Information("Logger configured");
        }
    }
}