using blogest.application.DTOs.responses.Saves;
using blogest.application.Features.commands.Saves;
using blogest.application.Interfaces.repositories.Saves;
using MediatR;

namespace blogest.application.Features.handlers.Saves
{
    public class AddSaveHandler : IRequestHandler<AddSaveCommand, AddSaveResponse>
    {
        private readonly ISavesCommandRepository _savesCommandRepository;
        public AddSaveHandler(ISavesCommandRepository savesCommandRepository)
        {
            _savesCommandRepository = savesCommandRepository;
        }
        public async Task<AddSaveResponse> Handle(AddSaveCommand request, CancellationToken cancellationToken)
        {
            AddSaveResponse response = await _savesCommandRepository.AddSaveOnPost(request.postId);

            return response;
        }
    }
}