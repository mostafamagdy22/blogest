using MediatR;

namespace blogest.application.Features.queries.Posts;

/// <summary>
/// Query to get paginated posts for a user, with optional includes.
/// </summary>
/// <param name="userId">The user ID.</param>
/// <param name="include">Related entities to include (e.g., comments).</param>
/// <param name="pageNumber">Page number for pagination.</param>
/// <param name="pageSize">Page size for pagination.</param>
public record GetPostsByUserIdQuery(Guid userId,string? include,int pageNumber = 1,int pageSize = 10) : IRequest<GetPostsByCategoryResponse>;