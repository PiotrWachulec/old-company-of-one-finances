using API.Configuration.ExecutionContext;
using API.Configuration.Extensions;
using API.Configuration.Validation;
using API.Modules.UserAccess;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using BuildingBlocks.Infrastructure.Emails;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.UserAccess.Infrastructure.Configuration;
using Serilog;
using Serilog.Formatting.Compact;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private static ILogger _logger;
        private static ILogger _loggerForApi;
        
        private const string CompanyOfOneFinancesDbConnectionString = "ConnectionStrings:CompanyOfOneFinancesDb";

        public Startup(IConfiguration configuration)
        {
            ConfigureLogger();
            
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerDocumentation();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
            
            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new UserAccessAutofacModule());
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var container = app.ApplicationServices.GetAutofacRoot();
            
            InitializeModules(container);

            app.UseMiddleware<CorrelationMiddleware>();
            
            app.UseSwaggerDocumentation();

            if (env.IsDevelopment())
            {
                app.UseProblemDetails();
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

        private void InitializeModules(ILifetimeScope container)
        {
            var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);
            
            var emailsConfiguration = new EmailsConfiguration(_configuration["EmailsConfiguration:FromEmail"]);
            
            UserAccessStartup.Initialize(
                _configuration[CompanyOfOneFinancesDbConnectionString],
                executionContextAccessor,
                emailsConfiguration,
                null,
                _logger);
        }
    }
}    