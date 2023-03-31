using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //Init();
        }

        private void Init()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            Students.Add(new Student() {Login = "s", PasswordHash = passHashS});
            Students.Add(new Student() {Login = "s2", PasswordHash = passHashS});
            Tutors.Add(new Tutor() { Login = "t", PasswordHash = passHashS });
            Tutors.Add(new Tutor() { Login = "t2", PasswordHash = passHashS });
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasKey(c => new { c.Id });
        }

        public async Task<(IAccountEntity? User, string? Role)> FindUserAsync(string accLogin)
        {
            var studentsSearchRes = await Students.FirstOrDefaultAsync(e => e.Login == accLogin);
            var tutorsSearchRes = await Tutors.FirstOrDefaultAsync(e => e.Login == accLogin);

            IAccountEntity? user = studentsSearchRes != null ? studentsSearchRes : tutorsSearchRes;
            var role = studentsSearchRes != null ? "Student" : "Tutor";
            return (user, role);
        }

        public DbSet<Tutor> Tutors => Set<Tutor>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<LessonTemplate> LessonTemplates => Set<LessonTemplate>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<TutorAchievements> TutorAchievements => Set<TutorAchievements>();
        public DbSet<TutorWork> TutorWorks => Set<TutorWork>();

        // ץור מע 's'
        private string passHashS =
            "pIg20fAaHpUDZxibspj/8OxrP5kgjxYxpqn+jL4pNvEDeA+zbmItcVo1GpIS4ELmf3O0ELkV47qgkYeemrN2y/Fc6ZJfFdnDB2QH+vzVowG0kFTylw6GZdRvJSuuXTuyvdFerxvnNP+hM6mD9GGo+BUJxMb61G82tblDL5kowIfF+ihSQotRUwZTMHHUeI9D9x0t46byg6h+KHFHkkau9FaE5k/TEC22VdOkOII7CpU+D3JhSTMCUeBAVUmdZMpB";
    }
}
