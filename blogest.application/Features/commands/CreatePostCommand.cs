using blogest.application.DTOs.requests;
using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.commands
{
    public class CreatePostCommand : IRequest<CreatePostResponseDto>
    {
        public string Title { get; init; }
        public string Content { get; init; }
        public DateTime PublishDate { get; init; }
    }
}