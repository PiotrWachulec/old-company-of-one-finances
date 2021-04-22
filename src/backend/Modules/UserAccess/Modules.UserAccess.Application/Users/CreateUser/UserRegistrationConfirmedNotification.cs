using System;
using BuildingBlocks.Application.Events;
using Modules.UserAccess.Domain.UserRegistrations.Events;
using Newtonsoft.Json;

namespace Modules.UserAccess.Application.Users.CreateUser
{
    public class UserRegistrationConfirmedNotification : DomainNotificationBase<UserRegistrationConfirmedDomainEvent>
    {
        [JsonConstructor]
        public UserRegistrationConfirmedNotification(UserRegistrationConfirmedDomainEvent domainEvent, Guid id)
            : base(domainEvent, id)
        {
        }
    }
}