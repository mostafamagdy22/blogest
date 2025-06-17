using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using blogest.application.Interfaces.services;
using MediatR;

namespace blogest.application.Features.handlers;

public class RefreshTokenHandler : IRequestHandler<TokenRequestCommand,RefreshTokenResponse>
{
    private readonly IJwtService _jwtService;
    public RefreshTokenHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }
    public async Task<RefreshTokenResponse> Handle(TokenRequestCommand request, CancellationToken cancellationToken)
    {
        RefreshTokenResponse response = await _jwtService.RotateRefreshTokenAsync(request.refreshToken, DateTime.UtcNow.AddDays(8));

        return response;
    }
}