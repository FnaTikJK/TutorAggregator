using AutoMapper;
using DAL.Core;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly IMapper mapper;

        public StudentRepository(DataContext dataContext, IMapper mapper) : base(dataContext)
        {
            this.mapper = mapper;
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<Student?> GetByLoginAsync(string login)
        {
            return await Set.FirstOrDefaultAsync(e => e.Login == login);
        }

        public Student GetByLogin(string login)
        {
            return Set.FirstOrDefault(e => e.Login == login);
        }

        public async Task<IEnumerable<Student>> GetAllAsync(Guid id)
        {
            return await Set.ToListAsync();
        }

        public async Task AddAsync(Student student)
        { 
            await Set.AddAsync(student);
        }
    }
}
