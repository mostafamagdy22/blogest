using blogest.application.DTOs.responses.Categories;
using blogest.application.Features.commands.Categories;

namespace blogest.application.Interfaces.repositories.Categories;

public interface ICategoriesCommandRepository
{
    public Task<CreateCategoryResponse> createCategory(CreateCategoryCommand command);
}