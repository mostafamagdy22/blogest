using blogest.application.DTOs.responses.Categories;
using MediatR;

namespace blogest.application.Features.commands.Categories;

public record CreateCategoryCommand(string title) : IRequest<CreateCategoryResponse>;