using System;
using System.Collections.Generic;
using Modules.UserAccess.Application.Contracts;

namespace Modules.UserAccess.Application.Authorization
{
    public class GetUserPermissionsQuery : QueryBase, IQuery<List<UserPermissionDto>>
    {
        public Guid UserId { get; }
        
        public GetUserPermissionsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}