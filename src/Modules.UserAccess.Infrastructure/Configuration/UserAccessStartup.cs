using Autofac;
using BuildingBlocks.Application;
using Serilog;

namespace Modules.UserAccess.Infrastructure.Configuration
{
    public class UserAccessStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            
            ILogger logger)
        {
            var loggerModule = logger.ForContext("Module", "UserAccess");
            
            ConfigureCompositionRoot(
                connectionString);
        }

        private static void ConfigureCompositionRoot(
            string connectionString)
        {
            var containerBuilder = new ContainerBuilder();


            _container = containerBuilder.Build();
            
            UserAccessCompositionRoot.SetContainer(_container);
        }
    }
}