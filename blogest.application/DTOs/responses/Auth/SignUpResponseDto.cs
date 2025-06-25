namespace blogest.application.DTOs.responses.Auth;

public class SignUpResponseDto
{
    public bool SignUpSuccessfully { get; set; }
    public Guid? UserId { get; set; }
    public string? Message { get; set; }
}