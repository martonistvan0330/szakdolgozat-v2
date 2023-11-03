namespace HomeworkManager.Model.CustomEntities.User;

public class UserListRow
{
    public required Guid UserId { get; set; }
    public required string FullName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string Roles { get; set; } = string.Empty;
}