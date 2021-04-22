using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Modules.UserAccess.Application.Contracts;
using Modules.UserAccess.Application.Users.GetUser;

namespace API.Modules.UserAccess
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public UserController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }
        
        [HttpGet("")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userDto = await _userAccessModule.ExecuteQueryAsync(new GetUserQuery());

            return Ok(userDto);
        }
    }
}