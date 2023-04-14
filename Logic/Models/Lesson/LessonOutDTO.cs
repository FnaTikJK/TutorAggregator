using Logic.Models.LessoonTemplate;
using Logic.Models.Student;
using Logic.Models.Tutor;

namespace Logic.Models.Lesson
{
    public class LessonOutDTO
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public string Place { get; set; }
        public TutorOutDTO Tutor { get; set; }
        public StudentOutDTO? Student { get; set; }
        public int? ChosenTemplateId { get; set; }
        public List<LessonTemplateOutDTO> AllowedTemplates { get; set; }
    }
}
