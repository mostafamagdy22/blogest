using blogest.application.DTOs.responses.Users;
using blogest.application.Features.queries.Users;
using MediatR;

namespace blogest.application.Features.handlers.Users;

public class GetUserInfoHandler : IRequestHandler<GetUserInfoQuery, GetUserInfoResponse>
{
    private readonly IUsersRepository _usersRepository;
    public GetUserInfoHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    public async Task<GetUserInfoResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        GetUserInfoResponse response = await _usersRepository.GetUserInfoById(request.userId, request.Include, request.PageNumber, request.PageSize);
        return response;
    }
}
