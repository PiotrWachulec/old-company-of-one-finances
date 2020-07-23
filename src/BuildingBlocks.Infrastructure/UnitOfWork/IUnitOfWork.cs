using System;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default,
            Guid? internalCommandId = null);
    }
}