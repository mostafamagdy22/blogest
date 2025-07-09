using Microsoft.AspNetCore.Mvc;

namespace blogest.api.DTO;

public class UploadImageDto
{
    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = default!;
}