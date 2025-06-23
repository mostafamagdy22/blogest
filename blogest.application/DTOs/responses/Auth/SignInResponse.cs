namespace blogest.application.DTOs.responses.Auth;

public record SignInResponse(string message,bool isAuth,string? token,string? refreshToken);