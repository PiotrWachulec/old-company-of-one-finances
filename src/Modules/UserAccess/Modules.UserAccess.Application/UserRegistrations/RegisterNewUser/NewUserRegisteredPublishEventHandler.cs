using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.EventBus;
using MediatR;
using Modules.UserAccess.IntegrationEvents;

namespace Modules.UserAccess.Application.UserRegistrations.RegisterNewUser
{
    public class NewUserRegisteredPublishEventHandler : INotificationHandler<NewUserRegisteredNotification>
    {
        private readonly IEventBus _eventBus;

        public NewUserRegisteredPublishEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(NewUserRegisteredNotification notification, CancellationToken cancellationToken)
        {
            _eventBus.Publish(new NewUserRegisteredIntegrationEvent(notification.Id, notification.DomainEvent.OccurredOn,
                notification.DomainEvent.UserRegistrationId.Value,
                notification.DomainEvent.Login,
                notification.DomainEvent.Email,
                notification.DomainEvent.FirstName,
                notification.DomainEvent.LastName,
                notification.DomainEvent.Name));

            return Task.CompletedTask;
        }
    }
}