using DAL.Core;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LessonTemplatesRepository : Repository<LessonTemplate>, ILessonTemplatesRepository
    {
        public LessonTemplatesRepository(DataContext dataContext) : base(dataContext)
        {
        }

        
        public async Task<IEnumerable<LessonTemplate>> GetAllByTutorAsync(string tutorLogin)
        {
            return await Set
                .Include(e => e.Tutor)
                .Where(e => e.Tutor.Login == tutorLogin)
                .ToArrayAsync();
        }

        public async Task<LessonTemplate?> GetByIdAsync(int id)
        {
            return await Set
                .Include(e => e.Tutor)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public LessonTemplate? GetById(int id)
        {
            return Set
                .Include(e => e.Tutor)
                .FirstOrDefault(e => e.Id == id);
        }

        public async Task AddAsync(LessonTemplate template)
        {
            await Set.AddAsync(template);
        }

        public void Delete(LessonTemplate entity)
        {
            if (entity != null)
                Set.Remove(entity);
        }
    }
}
