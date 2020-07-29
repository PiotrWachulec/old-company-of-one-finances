using System;
using System.Threading;
using System.Threading.Tasks;
using Modules.UserAccess.Application.Authentication;
using Modules.UserAccess.Application.Configuration.Commands;
using Modules.UserAccess.Domain.UserRegistrations;

namespace Modules.UserAccess.Application.UserRegistrations.RegisterNewUser
{
    internal class RegisterNewUserCommandHandler : ICommandHandler<RegisterNewUserCommand, Guid>
    {
        private readonly IUserRegistrationRepository _userRegistrationRepository;
        private readonly IUsersCounter _usersCounter;

        public RegisterNewUserCommandHandler(
            IUserRegistrationRepository userRegistrationRepository, 
            IUsersCounter usersCounter)
        {
            _userRegistrationRepository = userRegistrationRepository;
            _usersCounter = usersCounter;
        }

        public async Task<Guid> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            var password = PasswordManager.HashPassword(request.Password);

            var userRegistration = UserRegistration.RegisterNewUser(
                request.Login, 
                password, 
                request.Email, 
                request.FirstName,
                request.LastName,
                _usersCounter);

            await _userRegistrationRepository.AddAsync(userRegistration);

            return userRegistration.Id.Value;
        }
    }
}