using API.Configuration.Authorization;
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
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.UserAccess.Application.IdentityServer;
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
        
        
        // Configuration keys
        private const string CompanyOfOneFinancesDbConnectionString = "ConnectionStrings:CompanyOfOneFinancesDb";
        private const string EmailConfigurationFromEmail = "EmailsConfiguration:FromEmail";
        
        private const string IdentityServerClientId = "IdentityServerConfiguration:ClientId";
        private const string IdentityServerClientSecret = "IdentityServerConfiguration:ClientSecret";
        private const string IdentityServerScopeName = "IdentityServerConfiguration:ScopeName";
        private const string IdentityServerScopeDisplayName = "IdentityServerConfiguration:ScopeDisplayName";
        private const string IdentityServerAuthority = "IdentityServerConfiguration:Authority";
        

        public Startup(IConfiguration configuration)
        {
            ConfigureLogger();
            
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerDocumentation();

            ConfigureIdentityServer(services);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
            
            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                    policyBuilder.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                });
            });

            services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
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

            app.UseIdentityServer();

            if (env.IsDevelopment())
            {
                app.UseProblemDetails();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private void ConfigureIdentityServer(IServiceCollection services)
        {
            var identityServerConfig = InitializeIdentityServerConfig();

            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfigProvider.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfigProvider.GetApis())
                .AddInMemoryClients(IdentityServerConfigProvider.GetClients())
                .AddInMemoryPersistedGrants()
                .AddProfileService<ProfileService>()
                .AddDeveloperSigningCredential();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, x =>
                {
                    x.Authority = identityServerConfig.Authority;
                    x.ApiName = identityServerConfig.ScopeName;
                    x.RequireHttpsMetadata = false;
                });
        }

        private IdentityServerConfig InitializeIdentityServerConfig()
        {
            IdentityServerConfig identityServerConfig = new IdentityServerConfig
            {
                ClientId = _configuration[IdentityServerClientId],
                ClientSecret = _configuration[IdentityServerClientSecret],
                ScopeName = _configuration[IdentityServerScopeName],
                ScopeDisplayName = _configuration[IdentityServerScopeDisplayName],
                Authority = _configuration[IdentityServerAuthority]
            };

            IdentityServerConfigProvider.Initialize(identityServerConfig);
            
            return identityServerConfig;
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
            
            var emailsConfiguration = new EmailsConfiguration(_configuration[EmailConfigurationFromEmail]);
            
            UserAccessStartup.Initialize(
                _configuration[CompanyOfOneFinancesDbConnectionString],
                executionContextAccessor,
                emailsConfiguration,
                null,
                _logger);
        }
    }
}    