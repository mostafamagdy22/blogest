using blogest.domain.Entities;

namespace blogest.application.Interfaces.repositories
{
    public interface IPostsRepository
    {
        public Task<Guid> AddAsync(Post post);        
    }
}