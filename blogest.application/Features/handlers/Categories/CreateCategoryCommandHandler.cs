using blogest.application.DTOs.responses.Categories;
using blogest.application.Features.commands.Categories;
using blogest.application.Interfaces.repositories.Categories;
using MediatR;

namespace blogest.application.Features.handlers.Categories;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    private readonly ICategoriesRepository _categoriesRepository;
    public CreateCategoryCommandHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }
    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        CreateCategoryResponse response = await _categoriesRepository.createCategory(request);

        return response;
    }
}