using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todoapp.server.Dtos.UserDto;
using todoapp.server.Services.iml;

namespace todoapp.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var response = await _authService.UserLogin(request, HttpContext);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest request)
        {
            var response =await _authService.UserSignUp(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }


    }
}
