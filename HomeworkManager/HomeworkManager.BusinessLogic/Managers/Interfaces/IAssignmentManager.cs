using FluentResults;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IAssignmentManager
{
    Task<bool> ExistsWithIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<bool> ExistsDraftWithIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<Result<Pageable<AssignmentListRow>>> GetAllByUserAsync(PageableOptions pageableOptions, CancellationToken cancellationToken);
    Task<Result<AssignmentModel>> GetModelAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<Result<int>> CreateAsync(NewAssignment newAssignment, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int assignmentId, UpdatedAssignment updatedAssignment, CancellationToken cancellationToken = default);
    Task<Result> PublishAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<bool> NameAvailableAsync(NewAssignment newAssignment, CancellationToken cancellationToken = default);
    Task<bool> NameAvailableAsync(int assignmentId, string name, CancellationToken cancellationToken = default);
    Task<bool> IsInGroupAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<bool> IsTeacherAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<bool> IsCreatorAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<Result<AssignmentTypeId>> GetAssignmentTypeIdAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssignmentType>> GetAssignmentTypes(CancellationToken cancellationToken = default);
}