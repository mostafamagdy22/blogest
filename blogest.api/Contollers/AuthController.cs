using blogest.application.DTOs.responses.Auth;
using blogest.application.Features.commands.Auth;
using blogest.application.Interfaces.services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        public AuthController(IMediator mediator, IAuthService authService)
        {
            _authService = authService;
            _mediator = mediator;
        }
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            string result = await _authService.LogOut();
            if (string.IsNullOrEmpty(result) || result.ToLower().Contains("error"))
                return BadRequest("Logout failed");
            return Ok(result);
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand signUpCommand)
        {
            SignUpResponseDto result = await _mediator.Send(signUpCommand);
            if (!result.SignUpSuccessfully)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand signInCommand)
        {
            SignInResponse result = await _mediator.Send(signInCommand);
            if (!result.isAuth)
                return Unauthorized(result);
            return Ok(result);
        }
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/" };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
    }
}