using System;
using Modules.UserAccess.Application.Configuration.Commands;

namespace Modules.UserAccess.Application.UserRegistrations.ExpireUserRegistration
{
    public class ExpireUserRegistrationCommand : InternalCommandBase
    {
        public Guid UserRegistrationId { get; }

        public ExpireUserRegistrationCommand(Guid id, Guid userRegistrationId) : base(id)
        {
            UserRegistrationId = userRegistrationId;
        }
    }
}