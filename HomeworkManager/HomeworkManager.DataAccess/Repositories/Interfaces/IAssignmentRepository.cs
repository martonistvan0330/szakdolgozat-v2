using HomeworkManager.Model.CustomEntities.Assignment;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IAssignmentRepository
{
    Task<int> CreateAsync(NewAssignment newAssignment, int groupId, Guid creatorId, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithNameAsync(int groupId, string name, CancellationToken cancellationToken = default);
}