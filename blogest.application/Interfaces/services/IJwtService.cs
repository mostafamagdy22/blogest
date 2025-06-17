
using blogest.application.DTOs.responses;

namespace blogest.application.Interfaces.services;

public interface IJwtService
{
    public string GenrateToken(Guid userId, string userName, IEnumerable<string> roles, int expiryMinutes = 60);
    public string GenerateRefreshToken();
    public Task<RefreshTokenResponse> RotateRefreshTokenAsync(string oldToken, DateTime newExpiryDate);
    public Task<bool> IsRefreshTokenValid(string refreshToken);
    public Task AddRefreshTokenToDb(string refreshToken, Guid userId);
}