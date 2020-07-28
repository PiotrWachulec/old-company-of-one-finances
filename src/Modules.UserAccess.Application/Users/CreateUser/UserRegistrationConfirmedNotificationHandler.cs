using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Modules.UserAccess.Application.Users.CreateUser
{
    public class UserRegistrationConfirmedNotificationHandler : INotificationHandler<UserRegistrationConfirmedNotification>
    {
        public Task Handle(UserRegistrationConfirmedNotification notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}