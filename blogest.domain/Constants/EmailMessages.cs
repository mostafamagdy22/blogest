namespace blogest.domain.Constants;

public static class EmailMessages
{
 public const string ResetPasswordSubject = "Reset Your Password";
        public const string ResetPasswordBodyTemplate = @"
            <h2>Reset Your Password</h2>
            <p>Hi,</p>
            <p>You requested to reset your password. Please click the link below to continue:</p>
            <p><a href='{0}'>Reset Password</a></p>
            <p>If you didn't request this, please ignore this email.</p>
        ";

        public const string ConfirmEmailSubject = "Confirm Your Email";
        public const string ConfirmEmailBodyTemplate = @"
            <h2>Email Confirmation</h2>
            <p>Hi,</p>
            <p>Please confirm your email address by clicking the link below:</p>
            <p><a href='{0}'>Confirm Email</a></p>
            <p>Thank you for using our service!</p>
        ";

        public const string WelcomeSubject = "Welcome to Blogest!";
        public const string WelcomeBody = @"
            <h2>Welcome!</h2>
            <p>Thanks for signing up. We’re excited to have you on board!</p>
        ";

        public const string PasswordChangedSubject = "Your Password Has Been Changed";
        public const string PasswordChangedBody = @"
            <h2>Password Changed</h2>
            <p>This is a confirmation that your password was successfully changed.</p>
            <p>If this wasn't you, please contact support immediately.</p>
        ";
}