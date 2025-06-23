using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.queries;

public record GetCommentByIdQuery(Guid CommentId) : IRequest<GetCommentByIdResponse>;