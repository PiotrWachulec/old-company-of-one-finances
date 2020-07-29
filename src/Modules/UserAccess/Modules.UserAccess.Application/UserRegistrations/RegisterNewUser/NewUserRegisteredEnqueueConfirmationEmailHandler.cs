using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modules.UserAccess.Application.Configuration.Commands;
using Modules.UserAccess.Application.UserRegistrations.SendUserRegistrationConfirmationEmail;

namespace Modules.UserAccess.Application.UserRegistrations.RegisterNewUser
{
    public class NewUserRegisteredEnqueueConfirmationEmailHandler : INotificationHandler<NewUserRegisteredNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public NewUserRegisteredEnqueueConfirmationEmailHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }
        
        public async Task Handle(NewUserRegisteredNotification notification, CancellationToken cancellationToken)
        {
            await _commandsScheduler.EnqueueAsync(new SendUserRegistrationConfirmationEmailCommand(
                Guid.NewGuid(),
                notification.DomainEvent.UserRegistrationId,
                notification.DomainEvent.Email));
        }
    }
}