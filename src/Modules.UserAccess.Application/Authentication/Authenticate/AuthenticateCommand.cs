using Modules.UserAccess.Application.Contracts;

namespace Modules.UserAccess.Application.Authentication.Authenticate
{
    public class AuthenticateCommand : CommandBase<AuthenticationResult>
    {
        public string Login { get; }
        public string Password { get; }

        public AuthenticateCommand(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}