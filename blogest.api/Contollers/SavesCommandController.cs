using blogest.application.DTOs.responses.Saves;
using blogest.application.Features.commands.Saves;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/[controller]")]
public class SavesCommandController : ControllerBase
{
    private readonly IMediator _mediator;
    public SavesCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("Add-Save/{postId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddSave([FromRoute] Guid postId)
    {
        AddSaveCommand command = new AddSaveCommand(postId);
        AddSaveResponse response = await _mediator.Send(command);
        if (!response.IsSuccess)
            return BadRequest(response.Message);

        return Ok(response.Message);
    }
    [HttpDelete("Delete-Save/{postId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteSave([FromRoute] Guid postId)
    {
        DeleteSaveCommand command = new DeleteSaveCommand(postId);
        DeleteSaveResponse response = await _mediator.Send(command);
        if (!response.IsSuccess)
            return BadRequest(response.Message);
        return Ok(response.Message);
    }
}