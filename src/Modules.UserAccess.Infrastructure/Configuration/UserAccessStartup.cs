using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Application.Emails;
using BuildingBlocks.Infrastructure.Emails;
using Modules.UserAccess.Infrastructure.Configuration.DataAccess;
using Modules.UserAccess.Infrastructure.Configuration.Domain;
using Modules.UserAccess.Infrastructure.Configuration.Email;
using Modules.UserAccess.Infrastructure.Configuration.EventBus;
using Modules.UserAccess.Infrastructure.Configuration.Logging;
using Modules.UserAccess.Infrastructure.Configuration.Processing;
using Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
using Modules.UserAccess.Infrastructure.Configuration.Quartz;
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
            var moduleLogger = logger.ForContext("Module", "UserAccess");
            
            ConfigureCompositionRoot(
                connectionString,
                executionContextAccessor,
                emailsConfiguration,
                emailSender,
                logger);

            QuartzStartup.Initialize(moduleLogger);

            EventBusStartup.Initialize(moduleLogger);
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
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new EmailModule(emailsConfiguration, emailSender));
            containerBuilder.RegisterModule(new EventBusModule());
            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "UserAccess")));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new OutboxModule());
            containerBuilder.RegisterModule(new ProcessingModule());
            containerBuilder.RegisterModule(new QuartzModule());

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();
            
            UserAccessCompositionRoot.SetContainer(_container);
        }
    }
}