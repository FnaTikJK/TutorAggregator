using DAL.Entities;
using DAL.Entities.Enums;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IStudentRepository studentRepository;

        public UserRepository(ITutorRepository tutorRepository, IStudentRepository studentRepository)
        {
            this.tutorRepository = tutorRepository;
            this.studentRepository = studentRepository;
        }


        public async Task<User?> GetUserAsync(string login)
        {
            var student = await studentRepository.GetByLoginAsync(login);
            var tutor = await  tutorRepository.GetByLoginAsync(login);
            User user;
            if (student != null)
                return new User()
                {
                    Login = student.Login,
                    PasswordHash = student.PasswordHash,
                    Role = Role.Student,
                };
            else if (tutor != null)
                return new User()
                {
                    Login = tutor.Login,
                    PasswordHash = tutor.PasswordHash,
                    Role = Role.Tutor,
                };
            else
                return null;
        }

        public async Task UpdateAndSaveAsync(User user)
        {
            var student = await studentRepository.GetByLoginAsync(user.Login);
            var tutor = await tutorRepository.GetByLoginAsync(user.Login);
            if (student != null)
            {
                student.PasswordHash = user.PasswordHash;
                await studentRepository.SaveChangesAsync();
            }
            else if (tutor != null)
            {
                tutor.PasswordHash = user.PasswordHash;
                await tutorRepository.SaveChangesAsync();
            }
        }
    }
}
