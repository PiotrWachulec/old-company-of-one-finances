using Autofac;
using BuildingBlocks.Application.Outbox;
using Modules.UserAccess.Infrastructure.OutboxMessages;

namespace Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
    internal class OutboxModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OutboxAccessor>()
                .As<IOutbox>()
                .FindConstructorsWith(new AllConstructorFinder())
                .InstancePerLifetimeScope();
        }
    }
}