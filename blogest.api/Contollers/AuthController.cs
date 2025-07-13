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
using Swashbuckle.AspNetCore.Annotations;

namespace blogest.api.Contollers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, IUsersRepository usersRepository, IMediator mediator, IAuthService authService)
        {
            _logger = logger;
            _usersRepository = usersRepository;
            _authService = authService;
            _mediator = mediator;
        }
        /// <summary>
        /// Logs out the current user and invalidates their session/token.
        /// </summary>
        /// <returns>200 if logout successful, 400 if failed.</returns>
        [HttpPost("LogOut")]
        [SwaggerResponse(StatusCodes.Status200OK, "if log out success")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "if log out fail")]
        ///<summary>
        /// log out from the system
        /// </summary>
        public async Task<IActionResult> LogOut()
        {
            string result = await _authService.LogOut();
            if (string.IsNullOrEmpty(result) || result.ToLower().Contains("error"))
                return BadRequest("Logout failed");
            return Ok(result);
        }
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="signUpCommand">User registration data.</param>
        /// <returns>200 if registration successful, 400 if failed.</returns>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand signUpCommand)
        {
            SignUpResponseDto result = await _mediator.Send(signUpCommand);
            if (!result.SignUpSuccessfully)
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="signInCommand">User login data.</param>
        /// <returns>200 if login successful, 401 if failed.</returns>
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand signInCommand)
        {
            _logger.LogInformation("Login started at {Time}",DateTime.UtcNow);
            SignInResponse result = await _mediator.Send(signInCommand);
            if (!result.isAuth)
                return Unauthorized(result);
            return Ok(result);
        }
        /// <summary>
        /// Initiates Google OAuth login flow.
        /// </summary>
        /// <returns>Redirects to Google login page.</returns>
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
        /// <summary>
        /// Handles Google OAuth callback and logs in/creates the user.
        /// </summary>
        /// <returns>200 if successful, 400/401 if failed.</returns>
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
                user = await _authService.CreateUserFromGoogleAsync(email, name, googleId);

            var jwt = await _authService.SignIn(new SignInCommand(email, null));

            return Ok();
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordCommand command)
        {
            ForgetPasswordResponse response = await _mediator.Send(command);
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }
        [HttpPost("forgot-password-callback")]
        public async Task<IActionResult> ForgotPasswordCallBack([FromBody]FrogetPasswordCallBackCommand command)
        {
            ForgetPasswordCallBackResponse response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}