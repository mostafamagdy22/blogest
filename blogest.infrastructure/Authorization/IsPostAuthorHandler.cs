using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace blogest.infrastructure.Authorization;

public class IsPostAuthorHandler : AuthorizationHandler<IsPostAuthorRequirement>
{
    private readonly IPostsQueryRepository _postsQueryRepository;
    public IsPostAuthorHandler(IPostsQueryRepository postsQueryRepository)
    {
        _postsQueryRepository = postsQueryRepository;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPostAuthorRequirement requirement)
    {
        var httpContext = (context.Resource as DefaultHttpContext) ??
                          ((AuthorizationFilterContext)context.Resource)?.HttpContext;

        if (httpContext == null)
            return;

        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return;

        var postIdStr = httpContext.Request.RouteValues["postId"]?.ToString();
        if (!Guid.TryParse(postIdStr,out Guid postId))
            return;

        var post = await _postsQueryRepository.GetPostByIdAsync(postId);
        if (post != null && post.UserId.ToString() == userId)
        {
            context.Succeed(requirement);
        }

    }
}