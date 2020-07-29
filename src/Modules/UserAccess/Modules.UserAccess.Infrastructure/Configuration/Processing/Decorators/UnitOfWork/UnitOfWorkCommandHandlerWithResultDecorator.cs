using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Modules.UserAccess.Application.Configuration.Commands;
using Modules.UserAccess.Application.Contracts;

namespace Modules.UserAccess.Infrastructure.Configuration.Processing.Decorators.UnitOfWork
{
    internal class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
    {
        private readonly ICommandHandler<T, TResult> _decorated;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserAccessContext _userAccessContext;

        public UnitOfWorkCommandHandlerWithResultDecorator(
            ICommandHandler<T, TResult> decorated, 
            IUnitOfWork unitOfWork, 
            UserAccessContext userAccessContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _userAccessContext = userAccessContext;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            var result = await _decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase<TResult>)
            {
                var internalCommand = await _userAccessContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
    }
}