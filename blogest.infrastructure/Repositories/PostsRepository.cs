using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogest.application.Interfaces.repositories;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly BlogCommandContext _commandContext;
        public PostsRepository(BlogCommandContext commandContext)
        {
            _commandContext = commandContext;
        }
        public async Task<Guid> AddAsync(Post post)
        {
            Guid postId = Guid.NewGuid();
            post.SetId(postId);
            await _commandContext.Posts.AddAsync(post);
            await _commandContext.SaveChangesAsync();
            return postId;
        }
    }
}