using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public abstract class Repository<TEntity>
        where TEntity : class
    {
        private DataContext dataContext;

        protected Repository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        protected DbSet<TEntity> Set => dataContext.Set<TEntity>();

        protected async Task SaveChangesAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
