using Autofac;
using BuildingBlocks.EventBus;
using BuildingBlocks.Infrastructure.EventBus;

namespace Modules.UserAccess.Infrastructure.Configuration.EventBus
{
    internal class EventBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryEventBusClient>()
                .As<IEventBus>()
                .SingleInstance();
        }
    }
}