using DAL.Entities;

namespace DAL.Interfaces
{
    public interface ILessonRepository
    {
        public Task AddAsync(Lesson lesson);
        public Task SaveChangesAsync();
    }
}
