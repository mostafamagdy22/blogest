using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavesQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SavesQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
    }
}