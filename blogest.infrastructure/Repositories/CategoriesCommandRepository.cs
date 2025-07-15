using blogest.application.DTOs.responses.Categories;
using blogest.application.Features.commands.Categories;
using blogest.application.Interfaces.repositories.Categories;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class CategoriesCommandRepository : ICategoriesCommandRepository
{
    private readonly BlogCommandContext _context;
    public CategoriesCommandRepository(BlogCommandContext context)
    {
        _context = context;
    }
    public async Task<CreateCategoryResponse> createCategory(CreateCategoryCommand command)
    {
        Category category = new Category(title: command.title);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();


        return new CreateCategoryResponse(categoryId: category.Id,true,"category added successfully");
    }
}