using blogest.application.DTOs.responses.Saves;
using blogest.application.Interfaces.repositories.Saves;
using blogest.domain.Constants;
using blogest.infrastructure.Identity;
using blogest.infrastructure.persistence;
using Microsoft.AspNetCore.Identity;

namespace blogest.infrastructure.Repositories;

public class SavesCommandRepository : ISavesCommandRepository
{
    private readonly BlogCommandContext _blogCommandContext;
    private readonly IUsersRepository _usersRepository;
    public SavesCommandRepository(IUsersRepository usersRepository, BlogCommandContext blogCommandContext)
    {
        _blogCommandContext = blogCommandContext;
        _usersRepository = usersRepository;
    }
    public async Task<AddSaveResponse> AddSaveOnPost(Guid postId)
    {
        Guid? userId = _usersRepository.GetUserIdFromCookies();
        if (userId == null)
            return new AddSaveResponse
            {
                IsSuccess = false,
                Message = ErrorMessages.Unauthorized
            };

        bool userExists = await _blogCommandContext.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return new AddSaveResponse
            {
                IsSuccess = false,
                Message = ErrorMessages.NotFound
            };

        bool alreadySaved = await _blogCommandContext.Saves.AnyAsync(
            s => s.PostId == postId && s.UserId == userId
        );

        if (alreadySaved)
            return new AddSaveResponse
            {
                IsSuccess = false,
                Message = ErrorMessages.DuplicateEntry
            };

        Save save = new Save
        {
            PostId = postId,
            UserId = (Guid)userId
        };

        await _blogCommandContext.Saves.AddAsync(save);
        int result = await _blogCommandContext.SaveChangesAsync();

        return new AddSaveResponse
        {
            IsSuccess = result == 1,
            Message = result == 1 ? SuccessMessages.Created : ErrorMessages.InternalServerError
        };
    }

    public async Task<DeleteSaveResponse> DeleteSaveOnPost(Guid postId)
    {
        Guid? userId = _usersRepository.GetUserIdFromCookies()
                        ?? throw new UnauthorizedAccessException("UserId claim missing");

        Save save = await _blogCommandContext.Saves.FindAsync(postId,userId);

        if (save == null)
            return new DeleteSaveResponse
            {
                IsSuccess = false,
                Message = ErrorMessages.NotFound
            };

        _blogCommandContext.Saves.Remove(save);
        int result = await _blogCommandContext.SaveChangesAsync();

        return new DeleteSaveResponse
        {
            IsSuccess = result == 1,
            Message = result == 1 ? SuccessMessages.Deleted : ErrorMessages.InternalServerError
        };
    }
}