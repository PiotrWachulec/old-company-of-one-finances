using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.UserRegistrations.Rules
{
    public class UserRegistrationCannotBeConfirmedMoreThanOnceRule : IBusinessRule
    {
        private readonly UserRegistrationStatus _actualUserRegistrationStatus;

        public UserRegistrationCannotBeConfirmedMoreThanOnceRule(UserRegistrationStatus actualUserRegistrationStatus)
        {
            _actualUserRegistrationStatus = actualUserRegistrationStatus;
        }

        public bool IsBroken() => _actualUserRegistrationStatus == UserRegistrationStatus.Confirmed;

        public string Message => "User Registration cannot be confirmed more than once";
    }
}