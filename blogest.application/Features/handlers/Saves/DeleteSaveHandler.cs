using blogest.application.DTOs.responses.Saves;
using blogest.application.Features.commands.Saves;
using blogest.application.Interfaces.repositories.Saves;
using MediatR;

namespace blogest.application.Features.handlers.Saves;

public class DeleteSaveHandler : IRequestHandler<DeleteSaveCommand, DeleteSaveResponse>
{
    private readonly ISavesCommandRepository _savesCommandRepository;
    public DeleteSaveHandler(ISavesCommandRepository savesCommandRepository)
    {
        _savesCommandRepository = savesCommandRepository;
    }
    public async Task<DeleteSaveResponse> Handle(DeleteSaveCommand request, CancellationToken cancellationToken)
    {
        DeleteSaveResponse response = await _savesCommandRepository.DeleteSaveOnPost(request.postId);

        return response;
    }
}