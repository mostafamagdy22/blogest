namespace blogest.application.DTOs.responses.Tokens;
public record RefreshTokenResponse(string? refreshToken,string? accessToken,bool success);