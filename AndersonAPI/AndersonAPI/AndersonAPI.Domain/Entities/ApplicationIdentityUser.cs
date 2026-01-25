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