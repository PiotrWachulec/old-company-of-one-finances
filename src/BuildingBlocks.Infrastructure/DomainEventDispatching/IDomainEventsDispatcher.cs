using System.Threading.Tasks;

namespace BuildingBlocks.Infrastructure.DomainEventDispatching
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}