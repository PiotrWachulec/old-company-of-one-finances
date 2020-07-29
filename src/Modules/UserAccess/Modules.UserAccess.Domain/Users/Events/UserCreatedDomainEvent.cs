using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.Users.Events
{
    public class UserCreatedDomainEvent : DomainEventBase
    {
        public UserId UserId { get; }
        
        public UserCreatedDomainEvent(UserId userId)
        {
            UserId = userId;
        }
    }
}