using DAL.Entities;

namespace DAL.Interfaces
{
    public interface ITutorRepository
    {
        public Task<Tutor?> GetByLoginAsync(string login);
        public Task AddAsync(Tutor student);
        public Task SaveChangesAsync();
        public Tutor? GetByLogin(string tutorLogin);
    }
}
