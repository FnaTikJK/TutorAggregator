using DAL.Entities;

namespace DAL.Interfaces
{
    public interface ILessonTemplatesRepository
    {
        public Task<IEnumerable<LessonTemplate>> GetAllByTutorAsync(string tutorLogin);
        public Task<LessonTemplate?> GetByIdAsync(int id);
        public LessonTemplate? GetById(int id);
        public Task AddAsync(LessonTemplate template);
        public void Delete(LessonTemplate template);
        public Task SaveChangesAsync();
    }
}
