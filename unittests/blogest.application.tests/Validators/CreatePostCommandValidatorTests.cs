using Xunit;
using blogest.application.Validation;
using blogest.application.Features.commands;

public class CreatePostCommandValidatorTests
{
    [Fact]
    public void Should_Fail_When_Title_Is_Empty()
    {
        var validator = new CreatePostCommandValidator();
        var command = new CreatePostCommand { Title = "", Content = "valid content", PublishDate = DateTime.UtcNow.AddDays(1) };
        var result = validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }
}
