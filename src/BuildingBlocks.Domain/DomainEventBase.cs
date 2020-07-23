using System;

namespace BuildingBlocks.Domain
{
    public class DomainEventBase : IDomainEvent
    {
        public Guid Id { get; }
        
        public DateTime OccurredOn { get; }

        public DomainEventBase(Guid id, DateTime occurredOn)
        {
            Id = id;
            OccurredOn = occurredOn;
        }
    }
}