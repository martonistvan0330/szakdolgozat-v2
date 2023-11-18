using FluentResults;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IAssignmentRepository
{
    Task<bool> ExistsWithIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<bool> ExistsDraftWithIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithNameAsync(int groupId, string name, CancellationToken cancellationToken = default);
    Task<AssignmentModel?> GetModelByIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<string?> GetNameByIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<int?> GetGroupIdByIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(NewAssignment newAssignment, int groupId, Guid creatorId, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int assignmentId, UpdatedAssignment updatedAssignment, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int assignmentId, UpdatedAssignment updatedAssignment, Guid userId, CancellationToken cancellationToken = default);
    Task<Result> PublishAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsCreatorAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssignmentType>> GetAssignmentTypes(CancellationToken cancellationToken = default);
}