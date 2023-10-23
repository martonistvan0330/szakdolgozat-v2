using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.Model.Entities;

public class User : IdentityUser<Guid>
{
    public ICollection<AccessToken> AccessTokens { get; set; } = new HashSet<AccessToken>();

    public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
}