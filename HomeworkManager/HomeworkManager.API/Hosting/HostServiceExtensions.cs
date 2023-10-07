using HomeworkManager.BusinessLogic.Managers;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.DataAccess.Repositories;
using HomeworkManager.DataAccess.Repositories.Interfaces;

namespace HomeworkManager.API.Hosting
{
    public static class HostServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IEntityRepository, EntityRepository>();
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            return services.AddScoped<IEntityManager, EntityManager>();
        }
    }
}
