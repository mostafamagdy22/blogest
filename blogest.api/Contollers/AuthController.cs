using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand signUpCommand)
        {
            SignUpResponseDto result = await _mediator.Send(signUpCommand);
            return Ok(result);
        }
    }
}