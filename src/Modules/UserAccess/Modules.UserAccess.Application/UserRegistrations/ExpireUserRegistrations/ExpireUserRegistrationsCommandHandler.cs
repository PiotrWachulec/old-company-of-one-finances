using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Data;
using Dapper;
using MediatR;
using Modules.UserAccess.Application.Configuration.Commands;
using Modules.UserAccess.Application.UserRegistrations.ExpireUserRegistration;
using Modules.UserAccess.Domain.SharedKernel;
using Modules.UserAccess.Domain.UserRegistrations;

namespace Modules.UserAccess.Application.UserRegistrations.ExpireUserRegistrations
{
    internal class ExpireUserRegistrationsCommandHandler : ICommandHandler<ExpireUserRegistrationsCommand>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly ICommandsScheduler _commandsScheduler;

        public ExpireUserRegistrationsCommandHandler(
            ISqlConnectionFactory sqlConnectionFactory, 
            ICommandsScheduler commandsScheduler)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _commandsScheduler = commandsScheduler;
        }

        public async Task<Unit> Handle(ExpireUserRegistrationsCommand request, CancellationToken cancellationToken)
        {
            const string sql = "SELECT " +
                               "[UserRegistration].[Id] " +
                               "FROM [users].[UserRegistrations] AS [UserRegistration] " +
                               "WHERE [UserRegistration].[RegisterDate] < @Date AND " +
                               "[UserRegistration].[StatusCode] = @Status";

            var connection = _sqlConnectionFactory.GetOpenConnection();

            var timeForConfirmation = TimeSpan.FromMinutes(15);
            var date = SystemClock.Now.Add(-timeForConfirmation);

            var expiredUserRegistrationIds = 
                await connection.QueryAsync<Guid>(sql, new
                {
                    Date = date,
                    Status = UserRegistrationStatus.WaitingForConfirmation.Code
                });

            var expiredUserRegistrationIdsList = expiredUserRegistrationIds.AsList();

            foreach (var expiredUserRegistrationId in expiredUserRegistrationIdsList)
            {
                await _commandsScheduler.EnqueueAsync(
                    new ExpireUserRegistrationCommand(
                        Guid.NewGuid(), 
                        expiredUserRegistrationId));
            }

            return Unit.Value;
        }
    }
}