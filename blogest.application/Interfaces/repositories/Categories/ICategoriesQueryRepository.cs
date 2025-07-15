namespace blogest.application.Interfaces.repositories.Categories;

public interface ICategoriesQueryRepository
{
    public Task<List<string>> GetCategoriesByPostId(Guid postId);
}