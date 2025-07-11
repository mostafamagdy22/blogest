using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using blogest.application.DTOs.responses.Users;
using blogest.application.Interfaces.repositories;
using blogest.infrastructure.Identity;
using blogest.infrastructure.persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace blogest.infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManager<AppUser> _userManger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly BlogCommandContext _blogCommandContext;
    public UsersRepository(BlogCommandContext blogCommandContext, IMapper mapper, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _blogCommandContext = blogCommandContext;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userManger = userManager;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        AppUser user = await _userManger.FindByEmailAsync(email);

        if (user == null)
            return null;

        User userDto = _mapper.Map<User>(user);
        return userDto;
    }

    public Guid? GetUserIdFromCookies()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null || !Guid.TryParse(claim.Value, out Guid userId))
            return null;

        return userId;
    }

    public async Task<GetUserInfoResponse> GetUserInfoById(Guid userId, string? include, int pageNumber = 1, int pageSize = 10)
    {
        AppUser? user;
        List<GetPostResponse>? paginatedPosts = null;

        if (!string.IsNullOrEmpty(include) && include.Contains("posts"))
        {
            user = await _blogCommandContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new GetUserInfoResponse
                {
                    IsSuccess = false,
                    Message = "No user found"
                };

            paginatedPosts = await _blogCommandContext.Posts
                            .Include(p => p.Comments)
                            .Where(p => p.UserId == userId)
                            .OrderByDescending(p => p.PublishedAt)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(p => new GetPostResponse
                            {
                                PostId = p.PostId,
                                Title = p.Title,
                                Content = p.Content,
                                PublishAt = p.PublishedAt,
                                UserId = p.UserId,
                                Publisher = user.UserName,
                                Comments = _mapper.Map<List<CommentDto>>(p.Comments)
                            }).ToListAsync();
        }
        else
        {
            user = await _blogCommandContext.Users.FindAsync(userId);
            if (user == null)
                return new GetUserInfoResponse
                {
                    IsSuccess = false,
                    Message = "User not Found"
                };
        }

        GetUserInfoResponse userDto = _mapper.Map<GetUserInfoResponse>(user);
        userDto.IsSuccess = true;
        userDto.Message = "User info returned successfully";
        userDto.PostsOfUser = paginatedPosts;

        return userDto;
    }

    public async Task<bool> IsEmailExit(string email)
    {
        AppUser appUser = await _userManger.FindByEmailAsync(email);
        if (appUser != null)
            return true;
        return false;
    }

    public async Task<bool> IsUserNameExit(string userName)
    {
        if (await _userManger.FindByNameAsync(userName) != null)
            return true;
        return false;
    }
}