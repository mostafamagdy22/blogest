using System.Security.Claims;
using blogest.application.DTOs.responses.Auth;
using blogest.application.Features.commands.Auth;
using blogest.application.Interfaces.repositories.Users;
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
        private readonly IUsersRepository _usersRepository;
        public AuthController(IUsersRepository usersRepository, IMediator mediator, IAuthService authService)
        {
            _usersRepository = usersRepository;
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
        [HttpGet("login-google")]
        public async Task<IActionResult> LoginWithGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback", "Auth"),
                Items =
                {
                    {"prompt","select_account"}
                }
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return Unauthorized();

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var googleId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email not providedd by google");
            }

            var user = await _usersRepository.GetUserByEmailAsync(email);
            if (user == null)
                user = await _authService.CreateUserFromGoogleAsync(email,name,googleId);

            var jwt = await _authService.SignIn(new SignInCommand(email,null));

            return Ok();
        }
    }
}