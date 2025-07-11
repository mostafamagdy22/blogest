namespace blogest.application.Validation.Auth;

public class ForgetPasswordCallBackValidator : AbstractValidator<FrogetPasswordCallBackCommand>
{
    public ForgetPasswordCallBackValidator()
    {
        RuleFor(r => r.NewPassword).Equal(r => r.ConfirmPassword);

        RuleFor(r => r.NewPassword).MinimumLength(7);

        RuleFor(r => r.Email).NotEmpty().EmailAddress().WithMessage("email required and should be in correct format");

        RuleFor(r => r.Token).NotEmpty().WithMessage("token is required");
    }
}