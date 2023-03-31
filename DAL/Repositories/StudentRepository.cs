using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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

        public async Task<Student> GetByIdAsync(Guid id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<Student> GetByLoginAsync(string login)
        {
            return await Set.FindAsync(login);
        }

        public async Task<IEnumerable<Student>> GetAllAsync(Guid id)
        {
            return await Set.ToListAsync();
        }

        public async Task UpdateOrInsertAsync(Student student)
        {
            var existed = await Set.FindAsync(student.Login);
            if (existed == null)
                await Set.AddAsync(student);
            else
                mapper.Map(student, existed);

            await SaveChangesAsync();
        }
    }
}
