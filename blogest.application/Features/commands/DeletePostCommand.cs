using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.commands;

public record DeletePostCommand(Guid postId) : IRequest<DeletePostResponse>;