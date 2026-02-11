using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Identity;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class ApplicationIdentityUser : IdentityUser<string>
    {
        public string? RefreshToken { get; private set; }

        public DateTime? RefreshTokenExpired { get; private set; }

        public string? Name { get; private set; }

        public string? EmailConfirmationCode { get; private set; }

        public string? PasswordResetToken { get; private set; }

        public void SetEmailConfirmationCode(string? emailConfirmationCode)
        {
            EmailConfirmationCode = emailConfirmationCode;
        }

        public bool VerifyEmailCode(string code)
        {
            if (EmailConfirmationCode == null)
            {
                return false;
            }

            if (EmailConfirmationCode == code)
            {
                EmailConfirmationCode = null;
                EmailConfirmed = true;
                return true;
            }

            return false;
        }

        public void SetPasswordToken(string? passwordResetToken)
        {
            PasswordResetToken = passwordResetToken;
        }

        public bool VerifyPasswordToken(string token)
        {
            if (PasswordResetToken == null)
            {
                return false;
            }

            if (PasswordResetToken == token)
            {
                PasswordResetToken = null;
                return true;
            } 
            
            return false;
        }

        [IntentIgnore]
        public void UpdateRefreshToken(string? refreshToken, DateTime? refreshTokenExpired)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpired = refreshTokenExpired;
        }

        [IntentIgnore]
        public void SetName(string? name)
        {
            Name = name;
        }
    }
}