using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.Users.Events
{
    public class UserCreatedDomainEvent : DomainEventBase
    {
        public UserId Id { get; }
        
        public UserCreatedDomainEvent(UserId id)
        {
            Id = id;
        }
    }
}