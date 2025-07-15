using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogest.application.DTOs.responses.Search;
using blogest.application.Features.queries.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    /// <summary>
    /// Controller for searching posts, comments, or users.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance for handling queries.</param>
        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Searches posts by a given keyword.
        /// </summary>
        /// <param name="keyword">The search keyword.</param>
        /// <param name="pageNumber">Page number (default 1).</param>
        /// <param name="pageSize">Page size (default 10).</param>
        /// <returns>Paginated list of posts matching the keyword.</returns>
        [HttpGet("posts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> SearchPosts([FromQuery]string field,[FromQuery] string keyword, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            SearchQuery query = new SearchQuery(field,keyword, pageNumber, pageSize);
            SearchQueryResponse response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}