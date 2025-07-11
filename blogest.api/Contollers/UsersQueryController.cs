using blogest.application.DTOs.responses.Users;
using blogest.application.Features.queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("get-user-info/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserInfo([FromRoute] Guid userId, [FromQuery] string? include, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            GetUserInfoQuery query = new GetUserInfoQuery(userId, include, pageNumber, pageSize);
            GetUserInfoResponse response = await _mediator.Send(query);
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }
    }
}