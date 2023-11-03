using HomeworkManager.Model.CustomEntities.Role;

namespace HomeworkManager.Model.CustomEntities.User;

public class UserModel
{
    public required Guid UserId { get; set; }
    public required string FullName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required bool EmailConfirmed { get; set; }
    public ICollection<RoleModel> Roles { get; set; } = new HashSet<RoleModel>();
}