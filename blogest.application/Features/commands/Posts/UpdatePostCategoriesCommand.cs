using MediatR;

namespace blogest.application.Features.commands.Posts;

public record UpdatePostCategoriesCommand(Guid postId,List<int> categoryIds) : IRequest<UpdatePostCategoriesResponse>;