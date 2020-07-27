using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Application.Emails;
using BuildingBlocks.Infrastructure.Emails;
using Modules.UserAccess.Infrastructure.Configuration.Email;
using Serilog;

namespace Modules.UserAccess.Infrastructure.Configuration
{
    public class UserAccessStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            EmailsConfiguration emailsConfiguration,
            IEmailSender emailSender,
            ILogger logger)
        {
            var loggerModule = logger.ForContext("Module", "UserAccess");
            
            ConfigureCompositionRoot(
                connectionString,
                executionContextAccessor,
                emailsConfiguration,
                emailSender,
                logger);
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            EmailsConfiguration emailsConfiguration,
            IEmailSender emailSender,
            ILogger logger)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new EmailModule(emailsConfiguration, emailSender));

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();
            
            UserAccessCompositionRoot.SetContainer(_container);
        }
    }
}