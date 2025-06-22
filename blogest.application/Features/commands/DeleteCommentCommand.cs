using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.commands;

public record DeleteCommentCommand(Guid CommentId) : IRequest<DeleteCommentResponse>;