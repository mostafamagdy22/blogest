using blogest.application.Interfaces.repositories.Categories;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class CategoriesQueryRepository : ICategoriesQueryRepository
{
    private readonly BlogCommandContext _blogCommandContext;
    public CategoriesQueryRepository(BlogCommandContext blogCommandContext)
    {
        _blogCommandContext = blogCommandContext;
    }
    public async Task<List<string>> GetCategoriesByPostId(Guid postId)
    {
        List<string> categories = await _blogCommandContext.Categories.Include(c => c.PostCategories)
        .Where(c => c.PostCategories.Any(pc => pc.PostId == postId)).Select(c => c.Title).ToListAsync();

        return categories;
    }
}