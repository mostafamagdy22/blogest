using blogest.application.DTOs.responses.Tokens;
using blogest.application.Features.commands.Tokens;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly Mediator _mediator;
        public TokensController(Mediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Refreshes the JWT access token using a valid refresh token.
        /// </summary>
        /// <param name="request">Refresh token request data.</param>
        /// <returns>New access token if successful, 400 if failed.</returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh(TokenRequestCommand request)
        {
            RefreshTokenResponse result = await _mediator.Send(request);
            if (result is { success: false })
                return BadRequest(result);
            return Ok(result);
        }
    }
}