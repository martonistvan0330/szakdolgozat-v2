using HomeworkManager.Model.CustomEntities.Submission;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Extensions;

public static class SubmissionRepositoryExtensions
{
    public static IQueryable<SubmissionListRow> ToListModel(this IQueryable<Submission> submissions)
    {
        return submissions
            .Select(s => new SubmissionListRow
            {
                SubmissionId = s.SubmissionId,
                StudentId = s.StudentId,
                StudentName = s.Student.FullName,
                SubmittedAt = s.SubmittedAt!.Value
            });
    }

    public static IQueryable<Submission> Search(this IQueryable<Submission> submissions, string? searchText = null)
    {
        if (searchText is null)
        {
            return submissions;
        }

        return submissions.Where(u => u.Student.FullName.Contains(searchText));
    }
}