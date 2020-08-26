using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace UI
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
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
        private static void ConfigureLogger()
        {
            _logger =  new LoggerConfiguration()   
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.RollingFile(new CompactJsonFormatter(),"logs/logs")
                .CreateLogger();

            _loggerForApi = _logger.ForContext("Module", "UI");

            _loggerForApi.Information("Logger configured");
        }
    }
}