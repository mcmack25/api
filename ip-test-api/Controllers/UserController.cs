using ip_test_api.Controllers.Abstractions;
using ip_test_api.Models;
using ip_test_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ip_test_api.Controllers
{
    [Route("users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
            => GetHandledResult(await _userService.GetUsersAsync(cancellationToken));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
            => GetHandledResult(await _userService.GetUserAsync(id, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto user, CancellationToken cancellationToken)
            => GetHandledResult(await _userService.AddUserAsync(user, cancellationToken));

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto user, CancellationToken cancellationToken)
            => GetHandledResult(await _userService.UpdateUserAsync(id, user, cancellationToken));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) 
            => GetHandledResult(await _userService.DeleteUserAsync(id, cancellationToken));
    }
}
