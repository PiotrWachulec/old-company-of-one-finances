using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.UserRegistrations.Rules
{
    public class UserRegistrationCannotBeConfirmedAfterExpirationRule : IBusinessRule
    {
        private readonly UserRegistrationStatus _actualUserRegistrationStatus;

        public UserRegistrationCannotBeConfirmedAfterExpirationRule(UserRegistrationStatus actualUserRegistrationStatus)
        {
            _actualUserRegistrationStatus = actualUserRegistrationStatus;
        }

        public bool IsBroken() => _actualUserRegistrationStatus == UserRegistrationStatus.Expired;

        public string Message => "User Registration cannot be confirmed because it is expired";
    }
}