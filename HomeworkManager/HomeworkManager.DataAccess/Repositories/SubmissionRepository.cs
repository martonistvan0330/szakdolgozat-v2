using System.Linq.Expressions;
using FluentResults;
using HomeworkManager.DataAccess.Repositories.Extensions;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Submission;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.CustomEntities.Submission;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class SubmissionRepository : ISubmissionRepository
{
    private readonly HomeworkManagerContext _context;

    public SubmissionRepository(HomeworkManagerContext context)
    {
        _context = context;
    }
    
    public async Task<int> GetCountByAssignmentIdAsync(int assignmentId, string? searchText = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.Submissions
            .Where(s => s.AssignmentId == assignmentId)
            .Search(searchText)
            .CountAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<SubmissionListRow>> GetAllByAssignmentIdAsync<TKey>
    (
        int assignmentId,
        PageData? pageData = null,
        Expression<Func<SubmissionListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Submissions
            .Where(s => s.AssignmentId == assignmentId)
            .Search(searchText)
            .ToListModel()
            .OrderByWithDirection(orderBy, sortDirection)
            .GetPage(pageData)
            .ToListAsync(cancellationToken);
    }

    public async Task<TextSubmissionModel?> GetTextSubmissionAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.TextSubmissions
            .Where(s => s.AssignmentId == assignmentId && s.StudentId == userId)
            .Select(s => new TextSubmissionModel
            {
                SubmissionId = s.SubmissionId,
                StudentName = s.Student.FullName,
                SubmittedAt = s.SubmittedAt.HasValue ? s.SubmittedAt.Value.ToString("MM/dd/yyyy HH:mm:ss") : null,
                Answer = s.Answer,
                IsDraft = s.IsDraft
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<int> UpsertTextSubmissionAsync(UpdatedTextSubmission updatedTextSubmission, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var textSubmission = await _context.TextSubmissions
            .Where(s => s.AssignmentId == updatedTextSubmission.AssignmentId
                        && s.StudentId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (textSubmission == null)
        {
            textSubmission = new TextSubmission
            {
                AssignmentId = updatedTextSubmission.AssignmentId,
                StudentId = userId
            };

            _context.TextSubmissions.Add(textSubmission);
        }

        textSubmission.Answer = updatedTextSubmission.Answer;
        await _context.SaveChangesAsync(cancellationToken);

        return textSubmission.SubmissionId;
    }

    public async Task<Result> SubmitAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default)
    {
        var submission = await _context.Submissions
            .Where(s => s.AssignmentId == assignmentId && s.StudentId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (submission is null)
        {
            return new NotFoundError(SubmissionErrorMessages.SUBMISSION_WITH_ID_NOT_FOUND);
        }

        submission.IsDraft = false;
        submission.SubmittedAt = DateTime.Now;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<int?> GetAssignmentIdAsync(int submissionId, CancellationToken cancellationToken = default)
    {
        return await _context.Submissions
            .Where(s => s.SubmissionId == submissionId)
            .Select(s => s.Assignment.AssignmentId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}