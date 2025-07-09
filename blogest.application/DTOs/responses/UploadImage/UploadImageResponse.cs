namespace blogest.application.DTOs.responses.UploadImage;

public record UploadImageResponse(string? Url,bool IsSuccess,string? ErrorMessage);