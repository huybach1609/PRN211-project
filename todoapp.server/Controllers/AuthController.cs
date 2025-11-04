using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todoapp.server.Dtos.UserDtos;
using todoapp.server.Services.Implementations;
using todoapp.server.Services.Interfaces;

namespace todoapp.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> Login([FromBody] UserLoginRequest request, CancellationToken ct)
        {
            var response = await _authService.LoginAsync(request, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("signup")]
        public async Task<ActionResult<UserSignUpResponse>> SignUp([FromBody] UserSignUpRequest request, CancellationToken ct)
        {
            var response = await _authService.SignUpAsync(request, ct);
            return response.Success ? Ok(response) : BadRequest(response);
        }


    }
}
