using blogest.application.DTOs.responses.Users;
using MediatR;

namespace blogest.application.Features.queries.Users;

public record GetUserInfoQuery(Guid userId,string? Include,int PageNumber = 1,int PageSize = 10) : IRequest<GetUserInfoResponse>;