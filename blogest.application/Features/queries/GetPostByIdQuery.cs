using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.queries;

public record GetPostByIdQuery(Guid postId) : IRequest<GetPostResponse>;