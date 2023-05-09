using eShop.Application.System.Users;
using eShop.ViewModels.Catalog.Products.Dtos;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _userService.Authenticate(request);
            if (string.IsNullOrEmpty(token)) return BadRequest("Username or password is incorrect.");

            return Ok(token);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _userService.Register(request);
            if (!success) return BadRequest("Register is unsuccessful");

            return Ok();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUsersPagingRequest request)
        {
            var users = await _userService.GetUsersPaging(request);
            return Ok(users);
        }
    }
}