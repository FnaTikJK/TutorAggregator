using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IStudentRepository
    {
        public Task<Student> GetByIdAsync(Guid id);
        public Task<Student> GetByLoginAsync(string login);
        public Task<IEnumerable<Student>> GetAllAsync(Guid id);
        public Task UpdateOrInsertAsync(Student student);
    }
}
