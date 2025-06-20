using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.commands;

public record UpdatePostCommand(string? Title,string? Content,Guid postId) : IRequest<UpdatePostResponse>;