using blogest.application.DTOs.responses.Saves;
using MediatR;

namespace blogest.application.Features.commands.Saves;

public record AddSaveCommand(Guid postId) : IRequest<AddSaveResponse>;