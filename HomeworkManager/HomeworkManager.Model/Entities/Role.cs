using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.Model.Entities;

public class Role : IdentityRole<Guid>
{
    public int RoleId { get; set; }
}