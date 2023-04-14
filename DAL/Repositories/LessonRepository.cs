using DAL.Core;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task AddAsync(Lesson lesson)
        {
            await Set.AddAsync(lesson);
        }
    }
}
