using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.Model.Contexts;

public class HomeworkManagerContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<AccessToken> AccessTokens => Set<AccessToken>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentType> AssignmentTypes => Set<AssignmentType>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<EmailConfirmationToken> EmailConfirmationTokens => Set<EmailConfirmationToken>();
    public DbSet<Entity> Entities => Set<Entity>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<PasswordRecoveryToken> PasswordRecoveryTokens => Set<PasswordRecoveryToken>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public HomeworkManagerContext(DbContextOptions<HomeworkManagerContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Assignment>(entity =>
        {
            entity
                .Property(a => a.AssignmentTypeId)
                .HasConversion<int>();

            entity
                .HasOne(a => a.Creator)
                .WithMany()
                .HasForeignKey(a => a.CreatorId);
        });

        builder.Entity<AssignmentType>(entity =>
        {
            entity
                .Property(at => at.AssignmentTypeId)
                .HasConversion<int>();

            entity.HasData(
                Enum.GetValues(typeof(AssignmentTypeId))
                    .Cast<AssignmentTypeId>()
                    .Select(assignmentTypeId => new AssignmentType
                    {
                        AssignmentTypeId = assignmentTypeId,
                        Name = assignmentTypeId.ToString()
                    }));
        });

        builder.Entity<Course>(entity =>
        {
            entity.HasIndex(c => c.Name).IsUnique();

            entity
                .HasOne(c => c.Creator)
                .WithMany(u => u.CreatedCourses)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Group>(entity =>
        {
            entity.HasIndex(g => new
            {
                g.CourseId,
                g.Name
            }).IsUnique();

            entity
                .HasOne(g => g.Course)
                .WithMany(c => c.Groups)
                .HasForeignKey(g => g.CourseId);

            entity
                .HasOne(g => g.Creator)
                .WithMany(u => u.CreatedGroups)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<User>(entity =>
        {
            entity
                .HasMany(u => u.AttendedCourses)
                .WithMany(c => c.Students)
                .UsingEntity<AttendedCourse>(join =>
                {
                    join.ToTable("AttendedCourses");

                    join.HasOne(ac => ac.Course)
                        .WithMany()
                        .HasForeignKey(ac => ac.CourseId);

                    join.HasOne(ac => ac.User)
                        .WithMany()
                        .HasForeignKey(ac => ac.UserId);
                });

            entity
                .HasMany(u => u.ManagedCourses)
                .WithMany(c => c.Teachers)
                .UsingEntity<ManagedCourse>(join =>
                {
                    join.ToTable("ManagedCourses");

                    join.HasOne(mc => mc.Course)
                        .WithMany()
                        .HasForeignKey(mc => mc.CourseId);

                    join.HasOne(mc => mc.User)
                        .WithMany()
                        .HasForeignKey(mc => mc.UserId);
                });

            entity
                .HasMany(u => u.AttendedGroups)
                .WithMany(g => g.Students)
                .UsingEntity<AttendedGroup>(join =>
                {
                    join.ToTable("AttendedGroups");

                    join.HasOne(ag => ag.Group)
                        .WithMany()
                        .HasForeignKey(ag => ag.GroupId);

                    join.HasOne(ag => ag.User)
                        .WithMany()
                        .HasForeignKey(ag => ag.UserId);
                });

            entity
                .HasMany(u => u.ManagedGroups)
                .WithMany(g => g.Teachers)
                .UsingEntity<ManagedGroup>(join =>
                {
                    join.ToTable("ManagedGroups");

                    join.HasOne(mg => mg.Group)
                        .WithMany()
                        .HasForeignKey(mg => mg.GroupId);

                    join.HasOne(mg => mg.User)
                        .WithMany()
                        .HasForeignKey(mg => mg.UserId);
                });
        });
    }
}