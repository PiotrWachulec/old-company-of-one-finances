using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.UserAccess.Application.Contracts;
using Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

namespace API.Modules.UserAccess
{
    [ApiController]
    [Route("UserAccess/[controller]")]
    public class UserRegistrationsController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public UserRegistrationsController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }
        
        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> RegisterNewUser(RegisterNewUserRequest request)
        {
            await _userAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(request.Login, request.Password,
                request.Email, request.FirstName, request.LastName));

            return Ok();
        }
    }
}