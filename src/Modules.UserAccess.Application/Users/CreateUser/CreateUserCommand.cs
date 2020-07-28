using System;
using Modules.UserAccess.Application.Configuration.Commands;
using Modules.UserAccess.Domain.UserRegistrations;
using Newtonsoft.Json;

namespace Modules.UserAccess.Application.Users.CreateUser
{
    public class CreateUserCommand : InternalCommandBase<Guid>
    {
        public UserRegistrationId UserRegistrationId { get; }

        [JsonConstructor]
        public CreateUserCommand(Guid id, UserRegistrationId userRegistrationId) : base(id)
        {
            UserRegistrationId = userRegistrationId;
        }
    }
}