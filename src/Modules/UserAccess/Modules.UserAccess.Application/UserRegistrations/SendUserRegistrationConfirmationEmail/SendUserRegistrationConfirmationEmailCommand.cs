using System;
using Modules.UserAccess.Application.Configuration.Commands;
using Modules.UserAccess.Domain.UserRegistrations;
using Newtonsoft.Json;

namespace Modules.UserAccess.Application.UserRegistrations.SendUserRegistrationConfirmationEmail
{
    public class SendUserRegistrationConfirmationEmailCommand : InternalCommandBase
    {
        public UserRegistrationId UserRegistrationId { get; }
        public string Email { get; }
        
        [JsonConstructor]
        internal SendUserRegistrationConfirmationEmailCommand(
            Guid id, UserRegistrationId userRegistrationId, string email) : base(id)
        {
            UserRegistrationId = userRegistrationId;
            Email = email;
        }
    }
}