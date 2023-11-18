using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Extensions;

public static class AssignmentRepositoryExtensions
{
    public static IQueryable<AssignmentModel> ToModel(this IQueryable<Assignment> assignments)
    {
        return assignments
            .Select(a => new AssignmentModel
            {
                AssignmentId = a.AssignmentId,
                Name = a.Name,
                Description = a.Description,
                Deadline = a.Deadline,
                PresentationRequired = a.PresentationRequired,
                IsDraft = a.IsDraft,
                AssignmentTypeId = a.AssignmentTypeId,
                AssignmentTypeName = a.AssignmentType != null ? a.AssignmentType.Name : null,
                CourseId = a.Group.CourseId,
                CourseName = a.Group.Course.Name,
                GroupName = a.Group.Name
            });
    }

    public static IQueryable<AssignmentListRow> ToListModel(this IQueryable<Assignment> assignments)
    {
        return assignments
            .Select(a => new AssignmentListRow
            {
                AssignmentId = a.AssignmentId,
                Name = a.Name,
                Deadline = a.Deadline,
                IsDraft = a.IsDraft
            });
    }

    public static IQueryable<Assignment> Search(this IQueryable<Assignment> assignments, string? searchText = null)
    {
        if (searchText is null)
        {
            return assignments;
        }

        return assignments.Where(u => u.Name.Contains(searchText));
    }
}