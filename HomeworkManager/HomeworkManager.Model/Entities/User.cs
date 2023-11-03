using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.Model.Entities;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string FullName { get; set; }
    
    public ICollection<AccessToken> AccessTokens { get; set; } = new HashSet<AccessToken>();
}