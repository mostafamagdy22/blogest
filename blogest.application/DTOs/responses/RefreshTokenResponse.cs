namespace blogest.application.DTOs.responses;
public record RefreshTokenResponse(string? refreshToken,string? accessToken,bool success);