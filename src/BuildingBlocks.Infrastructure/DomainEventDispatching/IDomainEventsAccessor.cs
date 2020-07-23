using System.Collections.Generic;
using BuildingBlocks.Domain;

namespace BuildingBlocks.Infrastructure.DomainEventDispatching
{
    public interface IDomainEventsAccessor
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

        void ClearAllDomainEvents();
    }
}