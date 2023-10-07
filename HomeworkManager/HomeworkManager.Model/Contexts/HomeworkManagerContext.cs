using HomeworkManager.Model.Enitities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.Model.Contexts
{
    public class HomeworkManagerContext : DbContext
    {
        public HomeworkManagerContext(DbContextOptions<HomeworkManagerContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Entity> Entities { get; set; }
    }
}
