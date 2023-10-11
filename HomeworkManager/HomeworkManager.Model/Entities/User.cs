using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.Model.Entities
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}