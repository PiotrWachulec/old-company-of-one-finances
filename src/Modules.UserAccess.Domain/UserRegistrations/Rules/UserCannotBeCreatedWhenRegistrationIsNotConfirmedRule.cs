using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.UserRegistrations.Rules
{
    public class UserCannotBeCreatedWhenRegistrationIsNotConfirmedRule : IBusinessRule
    {
        private readonly UserRegistrationStatus _actualUserRegistrationStatus;

        internal UserCannotBeCreatedWhenRegistrationIsNotConfirmedRule(UserRegistrationStatus actualUserRegistrationStatus)
        {
            _actualUserRegistrationStatus = actualUserRegistrationStatus;
        }

        public bool IsBroken() => _actualUserRegistrationStatus != UserRegistrationStatus.Confirmed;
        
        public string Message => "User cannot be created when registration is not confirmed";
    }
}