using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Data;
using Dapper;
using Modules.UserAccess.Application.Configuration.Queries;

namespace Modules.UserAccess.Application.Authorization
{
    public class GetUserPermissionsQueryHandler : IQueryHandler<GetUserPermissionsQuery, List<UserPermissionDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetUserPermissionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        
        public async Task<List<UserPermissionDto>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT " +
                               "[UserPermission].[PermissionCode] AS [Code] " +
                               "FROM [users].[v_UserPermissions] AS [UserPermission] " +
                               "WHERE [UserPermission].UserId = @UserId";

            var permissions = await connection.QueryAsync<UserPermissionDto>(sql, new {request.UserId});

            return permissions.AsList();
        }
    }
}