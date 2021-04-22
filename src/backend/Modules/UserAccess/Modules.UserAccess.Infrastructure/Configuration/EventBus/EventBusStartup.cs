using Autofac;
using BuildingBlocks.Infrastructure.EventBus;
using Serilog;

namespace Modules.UserAccess.Infrastructure.Configuration.EventBus
{
    public static class EventBusStartup
    {
        public static void Initialize(ILogger logger)
        {
            SubscribeToIntegrationEvents(logger);
        }

        private static void SubscribeToIntegrationEvents(ILogger logger)
        {
            var eventBus = UserAccessCompositionRoot.BeginLifetimeScope().Resolve<IEventBus>();

            // Here I should add events from other modules which I want to observe in this module
        }

        private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger) where T:  IntegrationEvent
        {
            logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
            eventBus.Subscribe(
                new IntegrationEventGenericHandler<T>());
        }
    }
}