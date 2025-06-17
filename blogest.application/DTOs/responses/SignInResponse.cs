namespace blogest.application.DTOs.responses;

public record SignInResponse(string message,bool isAuth,string? token,string? refreshToken);