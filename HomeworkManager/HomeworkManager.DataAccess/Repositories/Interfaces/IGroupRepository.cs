using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IGroupRepository
{
    Task<bool> ExistsWithNameAsync(string groupName, int courseId);
    Task<IEnumerable<GroupListRow>> GetAllAsync(int courseId);
    Task<IEnumerable<GroupListRow>> GetAllByUserAsync(int courseId, Guid userId);
    Task<GroupModel?> GetModelAsync(int courseId, string groupName);
    Task<GroupModel?> GetModelByUserAsync(int courseId, string groupName, Guid userId);
    Task<Result<int, BusinessError>> CreateAsync(NewGroup newGroup, int courseId, User user);
    Task<BusinessError?> UpdateAsync(int courseId, string groupName, UpdateGroup updatedGroup, User? user = null);
    Task<bool> IsInGroupAsync(string groupName, int courseId, Guid userId);
    Task<bool> IsCreatorAsync(string groupName, int courseId, Guid userId);
    Task<bool> IsTeacherAsync(string groupName, int courseId, Guid userId);
}