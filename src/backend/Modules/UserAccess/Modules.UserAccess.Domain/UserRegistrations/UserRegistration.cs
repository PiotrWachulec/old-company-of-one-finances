using System;
using BuildingBlocks.Domain;
using Modules.UserAccess.Domain.UserRegistrations.Events;
using Modules.UserAccess.Domain.UserRegistrations.Rules;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistration : Entity, IAggregateRoot
    {
        public UserRegistrationId Id { get; private set; }

        private string _login;

        private string _password;

        private string _email;

        private string _firstName;

        private string _lastName;

        private string _name;

        private DateTime _registerDate;

        private UserRegistrationStatus _status;

        private DateTime? _confirmedDate;

        private UserRegistration()
        {
        }

        public static UserRegistration RegisterNewUser(
            string login, 
            string password, 
            string email, 
            string firstName,
            string lastName,
            IUsersCounter usersCounter)
        {
            return new UserRegistration(login, password, email, firstName, lastName, usersCounter);
        }

        private UserRegistration(
            string login, 
            string password, 
            string email, 
            string firstName, 
            string lastName,
            IUsersCounter usersCounter)
        {
            CheckRule(new UserLoginMustBeUniqueRule(usersCounter, login));

            Id = new UserRegistrationId(Guid.NewGuid());
            _login = login;
            _password = password;
            _email = email;
            _firstName = firstName;
            _lastName = lastName;
            _name = $"{firstName} {lastName}";
            _registerDate = DateTime.UtcNow;
            _status = UserRegistrationStatus.WaitingForConfirmation;

            AddDomainEvent(new NewUserRegisteredDomainEvent(Id, _login, _email, _firstName, _lastName, _name, _registerDate));
        }

        public User CreateUser()
        {
            CheckRule(new UserCannotBeCreatedWhenRegistrationIsNotConfirmedRule(_status));

            return User.CreateFromUserRegistration(Id, _login, _password, _email, _firstName, _lastName, _name);
        }

        public void Confirm()
        {
            CheckRule(new UserRegistrationCannotBeConfirmedMoreThanOnceRule(_status));
            CheckRule(new UserRegistrationCannotBeConfirmedAfterExpirationRule(_status));

            _status = UserRegistrationStatus.Confirmed;
            _confirmedDate = DateTime.UtcNow;

            AddDomainEvent(new UserRegistrationConfirmedDomainEvent(Id));
        }

        public void Expire()
        {
            CheckRule(new UserRegistrationCannotBeExpiredMoreThanOnceRule(_status));

            _status = UserRegistrationStatus.Expired;

            AddDomainEvent(new UserRegistrationExpiredDomainEvent(Id));
        }
    }
}