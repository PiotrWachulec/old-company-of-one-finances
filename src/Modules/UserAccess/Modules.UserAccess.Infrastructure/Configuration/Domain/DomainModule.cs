using Autofac;
using Modules.UserAccess.Application.UserRegistrations;
using Modules.UserAccess.Domain.UserRegistrations;

namespace Modules.UserAccess.Infrastructure.Configuration.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UsersCounter>()
                .As<IUsersCounter>()
                .InstancePerLifetimeScope();
        }
    }
}