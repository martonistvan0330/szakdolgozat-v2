using System.Linq.Expressions;
using FluentResults;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IAssignmentRepository
{
    Task<bool> ExistsWithIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<bool> ExistsDraftWithIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithNameAsync(int groupId, string name, CancellationToken cancellationToken = default);
    Task<int> GetCountByUserIdAsync(Guid userId, string? searchText = null, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<AssignmentListRow>> GetAllByUserIdAsync(
        Guid userId,
        PageData? pageData = null,
        string? searchText = null,
        CancellationToken cancellationToken = default
    )
    {
        return GetAllByUserIdAsync(userId, pageData, a => a.Deadline, searchText: searchText, cancellationToken: cancellationToken);
    }

    Task<IEnumerable<AssignmentListRow>> GetAllByUserIdAsync<TKey>(
        Guid userId,
        PageData? pageData = null,
        Expression<Func<AssignmentListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    );
    
    Task<AssignmentModel?> GetModelByIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<string?> GetNameByIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<int?> GetGroupIdByIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(NewAssignment newAssignment, int groupId, Guid creatorId, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int assignmentId, UpdatedAssignment updatedAssignment, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int assignmentId, UpdatedAssignment updatedAssignment, Guid userId, CancellationToken cancellationToken = default);
    Task<Result> PublishAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsCreatorAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default);
    Task<AssignmentTypeId?> GetAssignmentTypeIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssignmentType>> GetAssignmentTypes(CancellationToken cancellationToken = default);
}