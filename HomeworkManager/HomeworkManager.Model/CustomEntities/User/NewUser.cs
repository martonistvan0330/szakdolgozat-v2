namespace HomeworkManager.Model.CustomEntities.User;

public class NewUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}