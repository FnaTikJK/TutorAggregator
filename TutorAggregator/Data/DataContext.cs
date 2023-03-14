using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TutorAggregator.DataEntities;

namespace TutorAggregator.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
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
    }
}
