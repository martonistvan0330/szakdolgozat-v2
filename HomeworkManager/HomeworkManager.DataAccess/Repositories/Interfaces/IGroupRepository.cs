using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IGroupRepository
{
    Task<BusinessError?> CreateAsync(NewGroup newGroup, int courseId, User user);
}