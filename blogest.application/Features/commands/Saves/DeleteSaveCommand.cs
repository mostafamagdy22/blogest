using blogest.application.DTOs.responses.Saves;
using MediatR;

namespace blogest.application.Features.commands.Saves;

public record DeleteSaveCommand(Guid postId) : IRequest<DeleteSaveResponse>;