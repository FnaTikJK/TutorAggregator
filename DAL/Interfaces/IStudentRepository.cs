using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IStudentRepository
    {
        public Task<Student> GetByIdAsync(int id);
        public Task<Student?> GetByLoginAsync(string login);
        public Student GetByLogin(string login);
        public Task<IEnumerable<Student>> GetAllAsync(Guid id);
        public Task AddAsync(Student student);
        public Task SaveChangesAsync();
    }
}
