using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Application.Emails;
using BuildingBlocks.Infrastructure.Emails;
using Modules.UserAccess.Infrastructure.Configuration.DataAccess;
using Modules.UserAccess.Infrastructure.Configuration.Email;
using Modules.UserAccess.Infrastructure.Configuration.EventBus;
using Modules.UserAccess.Infrastructure.Configuration.Logging;
using Modules.UserAccess.Infrastructure.Mediation;
using Serilog;
using Serilog.Extensions.Logging;

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
            var loggerFactory = new SerilogLoggerFactory(logger);

            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            containerBuilder.RegisterModule(new EmailModule(emailsConfiguration, emailSender));
            containerBuilder.RegisterModule(new EventBusModule());
            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "UserAccess")));
            containerBuilder.RegisterModule(new MediatorModule());

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();
            
            UserAccessCompositionRoot.SetContainer(_container);
        }
    }
}