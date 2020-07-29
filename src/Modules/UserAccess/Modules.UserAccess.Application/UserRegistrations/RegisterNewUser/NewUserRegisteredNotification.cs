using System;
using BuildingBlocks.Application.Events;
using Modules.UserAccess.Domain.UserRegistrations.Events;
using Newtonsoft.Json;

namespace Modules.UserAccess.Application.UserRegistrations.RegisterNewUser
{
    public class NewUserRegisteredNotification : DomainNotificationBase<NewUserRegisteredDomainEvent>
    {
        [JsonConstructor]
        public NewUserRegisteredNotification(NewUserRegisteredDomainEvent domainEvent, Guid id) : base(domainEvent, id)
        {
        }
    }
}