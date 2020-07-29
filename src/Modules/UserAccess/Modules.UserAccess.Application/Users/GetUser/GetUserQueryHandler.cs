using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application;
using BuildingBlocks.Application.Data;
using Dapper;
using Modules.UserAccess.Application.Configuration.Queries;

namespace Modules.UserAccess.Application.Users.GetUser
{
    internal class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDto>
    {
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetUserQueryHandler(
            IExecutionContextAccessor executionContextAccessor,
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _executionContextAccessor = executionContextAccessor;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            var userId = _executionContextAccessor.UserId;

            const string sql = "SELECT" +
                               "[User].[Id], " +
                               "[User].[IsActive], " +
                               "[User].[Login], " +
                               "[User].[Email], " +
                               "[User].[Name] " +
                               "FROM [users].[v_Users] AS [User] " +
                               "WHERE [User].[Id] = @UserId";
            
            return await connection.QuerySingleAsync<UserDto>(sql, new
            {
                userId
            });
        }
    }
}