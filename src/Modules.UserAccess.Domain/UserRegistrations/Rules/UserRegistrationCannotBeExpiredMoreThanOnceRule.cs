using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.UserRegistrations.Rules
{
    public class UserRegistrationCannotBeExpiredMoreThanOnceRule : IBusinessRule
    {
        private readonly UserRegistrationStatus _actualUserRegistrationStatus;

        public UserRegistrationCannotBeExpiredMoreThanOnceRule(UserRegistrationStatus actualUserRegistrationStatus)
        {
            _actualUserRegistrationStatus = actualUserRegistrationStatus;
        }

        public bool IsBroken() => _actualUserRegistrationStatus == UserRegistrationStatus.Expired;

        public string Message => "User Registration cannot be expired more than once";
    }
}