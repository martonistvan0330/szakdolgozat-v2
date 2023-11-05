using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.Model.Entities;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string FullName { get; set; }
    
    public ICollection<AccessToken> AccessTokens { get; set; } = new HashSet<AccessToken>();
    public ICollection<Course> AttendedCourses { get; set; } = new HashSet<Course>();
    public ICollection<Course> ManagedCourses { get; set; } = new HashSet<Course>();
    public ICollection<Course> CreatedCourses { get; set; } = new HashSet<Course>();
    public ICollection<Group> AttendedGroups { get; set; } = new HashSet<Group>();
    public ICollection<Group> ManagedGroups { get; set; } = new HashSet<Group>();
    public ICollection<Group> CreatedGroups { get; set; } = new HashSet<Group>();
}