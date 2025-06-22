using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.commands;

public record UpdateCommentCommand(Guid CommentId,string NewContent) : IRequest<UpdateCommentResponse>;