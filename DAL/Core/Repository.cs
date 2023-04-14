using Microsoft.EntityFrameworkCore;

namespace DAL.Core
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

        public async Task SaveChangesAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
