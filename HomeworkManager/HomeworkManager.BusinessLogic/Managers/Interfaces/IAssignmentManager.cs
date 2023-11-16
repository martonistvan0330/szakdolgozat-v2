using FluentResults;
using HomeworkManager.Model.CustomEntities.Assignment;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IAssignmentManager
{
    Task<Result<int>> CreateAsync(NewAssignment newAssignment, CancellationToken cancellationToken = default);
    Task<bool> NameAvailableAsync(NewAssignment newAssignment, CancellationToken cancellationToken = default);
}