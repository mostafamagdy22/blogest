using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.commands;

public record CreateCommentCommand(string Content,Guid PostId,Guid UserId) : IRequest<CreateCommentResponse>;