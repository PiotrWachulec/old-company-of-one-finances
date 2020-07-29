using System;
using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.Users
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}