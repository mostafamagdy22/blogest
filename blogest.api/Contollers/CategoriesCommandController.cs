using blogest.application.DTOs.responses.Categories;
using blogest.application.Features.commands.Categories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoriesCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="command">The data required to create a new category.</param>
        /// <returns>Returns 200 if the category is created successfully, otherwise 400.</returns>
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryCommand command)
        {
            CreateCategoryResponse response = await _mediator.Send(command); 
            if (response is { IsSuccess: false })
                return BadRequest(response);
            return Ok(response);
        }
    }
}