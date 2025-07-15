using AutoMapper;
using blogest.application.Interfaces.repositories.Categories;
using blogest.application.Interfaces.services;
using blogest.domain.Constants;
using MediatR;

namespace blogest.application.Features.handlers.Posts;

public class CreatePostHandler : IRequestHandler<CreatePostCommand, CreatePostResponseDto>
{
    private readonly IPostsCommandRepository _postsRepository;
    private readonly IMapper _mapper;
    private readonly IUsersRepository _usersRepository;
    private readonly ISearchService _searchService;
    private readonly IPostsQueryRepository _postsQueryRepository;
    private readonly ICategoriesQueryRepository _categoriesQueryRepository;
    private readonly ICommentsQueryRepository _commentsQueryRepository;
    public CreatePostHandler(ICommentsQueryRepository commentsQueryRepository, ICategoriesQueryRepository categoriesQueryRepository, IPostsQueryRepository postsQueryRepository, ISearchService searchService, IUsersRepository usersRepository, IPostsCommandRepository postsRepository, IMapper mapper)
    {
        _commentsQueryRepository = commentsQueryRepository;
        _categoriesQueryRepository = categoriesQueryRepository;
        _postsQueryRepository = postsQueryRepository;
        _searchService = searchService;
        _usersRepository = usersRepository;
        _postsRepository = postsRepository;
        _mapper = mapper;
    }
    public async Task<CreatePostResponseDto> Handle(CreatePostCommand postCommand, CancellationToken cancellationToken)
    {
        if (postCommand.CategoryIds.Count == 0)
            return new CreatePostResponseDto(ErrorMessages.AddCategoryAtLeast, null, false);
        Post post = _mapper.Map<Post>(postCommand);
        await _postsRepository.AddAsync(post, postCommand.CategoryIds);

        var user = await _usersRepository.GetUserInfoById(post.UserId, null);
        List<string> categories = await _categoriesQueryRepository.GetCategoriesByPostId(post.PostId);
        GetPostResponse postDto = new GetPostResponse
        {
            Publisher = user.UserName!,
            PublishAt = post.PublishedAt,
            UserId = user.userId,
            Content = post.Content,
            Title = post.Title,
            CategoryNames = categories,
            Comments = null
        };

        await _searchService.IndexAsync<GetPostResponse>(postDto,"articles");

        if (post.PostId != null)
            return new CreatePostResponseDto(SuccessMessages.Created, post.PostId, true);
        else
            return new CreatePostResponseDto(ErrorMessages.BadRequest, null, false);
    }
}