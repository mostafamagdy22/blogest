using blogest.application.DTOs.responses.Saves;

namespace blogest.application.Interfaces.repositories.Saves;

public interface ISavesCommandRepository
{
    public Task<AddSaveResponse> AddSaveOnPost(Guid postId);
    public Task<DeleteSaveResponse> DeleteSaveOnPost(Guid postId);
}