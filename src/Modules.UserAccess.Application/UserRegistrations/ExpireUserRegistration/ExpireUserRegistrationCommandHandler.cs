using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Modules.UserAccess.Application.Configuration.Commands;
using Modules.UserAccess.Domain.UserRegistrations;

namespace Modules.UserAccess.Application.UserRegistrations.ExpireUserRegistration
{
    public class ExpireUserRegistrationCommandHandler : ICommandHandler<ExpireUserRegistrationCommand>
    {
        private readonly IUserRegistrationRepository _userRegistrationRepository;

        public ExpireUserRegistrationCommandHandler(IUserRegistrationRepository userRegistrationRepository)
        {
            _userRegistrationRepository = userRegistrationRepository;
        }

        public async Task<Unit> Handle(ExpireUserRegistrationCommand request, CancellationToken cancellationToken)
        {
            var userRegistration =
                await _userRegistrationRepository.GetByIdAsync(new UserRegistrationId(request.UserRegistrationId));
            
            userRegistration.Expire();
            
            return Unit.Value;
        }
    }
}