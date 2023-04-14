using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Place { get; set; }
        public Student? Student { get; set; }
        public int? ChosenTemplateId { get; set; }
        public List<LessonTemplate> AllowedTemplates { get; set; }
    }
}
