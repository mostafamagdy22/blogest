using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogest.application.Interfaces.repositories;
using blogest.infrastructure.persistence;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.ComponentModel.DataAnnotations;
namespace blogest.infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly BlogCommandContext _commandContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostsRepository(BlogCommandContext commandContext, IHttpContextAccessor httpContextAccessor)
        {
            _commandContext = commandContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Guid> AddAsync(Post post)
        {
            Guid postId = Guid.NewGuid();
            post.SetId(postId);

            var token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];

            if (token == null)
                throw new ValidationException("UnAuthoraized");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdCliam = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            var userId = userIdCliam?.Value;

            post.UserId = Guid.Parse(userId);

            await _commandContext.Posts.AddAsync(post);
            await _commandContext.SaveChangesAsync();
            return postId;
        }
    }
}