using blogest.application.DTOs.responses.Tokens;
using blogest.application.Features.commands.Tokens;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly Mediator _mediator;
        public TokensController(Mediator mediator)
        {
            _mediator = mediator;
        }
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