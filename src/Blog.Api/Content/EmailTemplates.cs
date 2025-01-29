namespace Blog.Api.Content
{
    public class EmailTemplates
    {
        public static string PasswordResetTemplate = @"
        <html>
        <body style='font-family: Arial, sans-serif; padding: 20px;'>
            <h2>Password Reset Request</h2>
            <p>Hello {0},</p>
            <p>We received a request to reset your password. Click the link below to set a new password:</p>
            <p>
                <a href='{1}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>
                    Reset Password
                </a>
            </p>
            <p>Or copy and paste this URL into your browser:</p>
            <p>{1}</p>
            <p>This link will expire in {2} hours.</p>
            <p>If you didn't request a password reset, please ignore this email.</p>
            <br>
            <p>Best regards,<br>Your Application Team</p>
        </body>
        </html>";

        public static string GeneratePasswordResetEmail(string userName, string resetUrl, int expiryHours = 24)
        {
            return string.Format(PasswordResetTemplate,
                userName,       
                resetUrl,        
                expiryHours 
            );
        }

        public static string EmailVerificationTemplate = @"
        Hello {0},

        Please click the link below to verify your email:

        {1}

        This link expires in {2} hours.

        Best regards,
        Your Team";

        public static string GenerateVerificationEmail(string userName, string verificationUrl, int expiryHours = 24)
        {
            return string.Format(EmailVerificationTemplate,
                userName,
                verificationUrl,
                expiryHours
            );
        }
    }
}
